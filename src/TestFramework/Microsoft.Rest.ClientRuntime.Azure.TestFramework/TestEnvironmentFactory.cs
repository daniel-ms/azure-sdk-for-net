﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Microsoft.Rest.ClientRuntime.Azure.TestFramework
{
    public static class TestEnvironmentFactory
    {        
        /// <summary>
        /// The environment variable name for CSM OrgId authentication
        /// 
        /// Sample Value 1 - Get token from user and password:
        /// TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};BaseUri=https://api-next.resources.windows-int.net/;UserId={user-id};Password={password}       
        /// 
        /// Sample Value 2 - Prompt for login credentials:
        /// TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId={subscription-id};AADAuthEndpoint=https://login.windows-ppe.net/;BaseUri=https://api-next.resources.windows-int.net/
        /// </summary>
        const string TestCSMOrgIdConnectionStringKey = "TEST_CSM_ORGID_AUTHENTICATION";

        /// <summary>
        /// Custom values that should override environment variables during runtime 
        /// </summary>
        public static Dictionary<string, string> CustomEnvValues = new Dictionary<string, string>();

        /// <summary>
        /// Return test credentials and URI using AAD auth for an OrgID account.  Use this method with caution, it may take a dependency on ADAL
        /// </summary>
        /// <returns>The test credentials, or null if the appropriate environment variable is not set.</returns>
        public static TestEnvironment GetTestEnvironment()
        {
            string connectionString = Environment.GetEnvironmentVariable(TestCSMOrgIdConnectionStringKey);
            TestEnvironment testEnv = new TestEnvironment(TestUtilities.ParseConnectionString(connectionString));

            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                testEnv.UserName = TestEnvironment.UserIdDefault;
                SetEnvironmentSubscriptionId(testEnv, connectionString);
                testEnv.TokenInfo = new TokenInfo(TestEnvironment.RawTokenDefault);
            }
            else //Record or None
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    IDictionary<string, string> parsedConnection = TestUtilities.ParseConnectionString(connectionString);

                    foreach (var keyVal in CustomEnvValues)
                    {
                        parsedConnection[keyVal.Key] = keyVal.Value;
                    }
                    testEnv = new TestEnvironment(parsedConnection);

                    if (parsedConnection.ContainsKey(TestEnvironment.RawToken))
                    {
                        var token = parsedConnection[TestEnvironment.RawToken];
                        testEnv.TokenInfo = new TokenInfo(token);
                    }
                    else
                    {
                        string password = null;
                        parsedConnection.TryGetValue(TestEnvironment.AADPasswordKey, out password);

                        if (testEnv.UserName != null && password != null)
                        {
                            var result = TestEnvironmentFactory.LoginUserAsync(testEnv.Tenant, 
                                testEnv.UserName, 
                                password, 
                                testEnv.Endpoints.AADTokenAudienceUri.ToString(),
                                testEnv.Endpoints.AADAuthUri.ToString())
                                .ConfigureAwait(false).GetAwaiter().GetResult();
                            testEnv.TokenInfo = new TokenInfo(result);
                        }
                        else if (testEnv.ServicePrincipal != null && password != null)
                        {
                            var result = TestEnvironmentFactory.LoginServicePrincipalAsync(testEnv.Tenant,
                                testEnv.ServicePrincipal, 
                                password, 
                                testEnv.Endpoints.AADTokenAudienceUri.ToString(),
                                testEnv.Endpoints.AADAuthUri.ToString())
                                .ConfigureAwait(false).GetAwaiter().GetResult();
                            testEnv.TokenInfo = new TokenInfo(result);
                        }
#if NET45
                        else
                        {
                            var result = LoginWithPromptAsync(testEnv.Tenant,
                                testEnv.Endpoints.AADTokenAudienceUri.ToString(),
                                testEnv.Endpoints.AADAuthUri.ToString())
                                .ConfigureAwait(false).GetAwaiter().GetResult();
                            testEnv.TokenInfo = new TokenInfo(result);
                        }
#endif
                    }
                }//end-of-if connectionString present

                // Management Clients that are not subscription based should set "SubscriptionId=None" in
                // the connection string.
                if (testEnv.SubscriptionId == null || !testEnv.SubscriptionId.Equals("None", StringComparison.OrdinalIgnoreCase))
                {
                    //Getting subscriptions from server
                    var subscriptions = ListSubscriptions(
                        testEnv.BaseUri.ToString(),
                        testEnv.TokenInfo);

                    if (subscriptions.Count == 0)
                    {
                        throw new Exception("Logged in account had no associated subscriptions. We are in " +
                                            testEnv.Endpoints.Name + " environment and the tenant is " +
                                            testEnv.Tenant + ". Please check if the subscription is in the " +
                                            "correct tenant and environment. You can set the envt. variable - " +
                                            "TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>;" +
                                            "Environment=<Env-name>;Tenant=<tenant-id>");
                    }

                    bool matchingSubscription = false;
                    //SubscriptionId is provided in envt. variable
                    if (testEnv.SubscriptionId != null)
                    {
                        matchingSubscription = subscriptions.Any(item => item.SubscriptionId == testEnv.SubscriptionId);
                        if (!matchingSubscription)
                        {
                            throw new Exception("The provided SubscriptionId in the envt. variable - \"" + testEnv.SubscriptionId +
                                                "\" does not match the list of subscriptions associated with this account.");
                        }
                    }
                    else
                    {
                        if (subscriptions.Count > 1)
                        {
                            throw new Exception("There are multiple subscriptions associated with the logged in account. " +
                                                "Please specify the subscription to use in the connection string. Please set " +
                                                "the envt. variable - TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>");
                        }
                        testEnv.SubscriptionId = subscriptions[0].SubscriptionId;
                    }
                }

                if (testEnv.SubscriptionId == null)
                {
                    throw new Exception("Subscription Id was not provided in environment variable. " + "Please set " + 
                                        "the envt. variable - TEST_CSM_ORGID_AUTHENTICATION=SubscriptionId=<subscription-id>. " + 
                                        "If SubscriptionId is not required for your management client then please set it to " +
                                        "'None' in the above mentioned envt. variable.");
                }

                SetEnvironmentSubscriptionId(testEnv, connectionString);
            }

            return testEnv;
        }

        public static void SetEnvironmentSubscriptionId(TestEnvironment testEnv, string connectionString)
        {
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                if (HttpMockServer.Variables.ContainsKey(TestEnvironment.SubscriptionIdKey))
                {
                    testEnv.SubscriptionId = HttpMockServer.Variables[TestEnvironment.SubscriptionIdKey];
                }
                else if (!string.IsNullOrEmpty(connectionString))
                {
                    IDictionary<string, string> parsedConnection = TestUtilities.ParseConnectionString(connectionString);
                    if (!parsedConnection.ContainsKey(TestEnvironment.SubscriptionIdKey))
                    {
                        throw new Exception(
                            "Subscription ID is not present in the recorded mock or in environment variables.");
                    }
                    testEnv.SubscriptionId = parsedConnection[TestEnvironment.SubscriptionIdKey];
                }
                else
                {
                    throw new Exception(
                        "Subscription ID is not present in the recorded mock or in environment variables.");
                }
            }
            else //Record or None
            {
                // Preserve/restore subscription ID
                HttpMockServer.Variables[TestEnvironment.SubscriptionIdKey] = testEnv.SubscriptionId;
            }
        }

        public static List<SubscriptionInfo> ListSubscriptions(string baseuri, TokenInfo token)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(string.Format("{0}/subscriptions?api-version=2014-04-01-preview", baseuri))
            };

            HttpClient client = new HttpClient();
            request.Headers.Authorization = new AuthenticationHeaderValue(token.AccessTokenType, 
                token.AccessToken);
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            string jsonString = response.Content.ReadAsStringAsync().Result;

            var jsonResult = JObject.Parse(jsonString);
            var results = ((JArray)jsonResult["value"]).Select(item => new SubscriptionInfo((JObject)item)).ToList();
            return results;
        }

        private static async Task<AuthenticationResult> LoginUserAsync(string domain, 
            string username, 
            string password,
            string resource,
            string authenticationEndpoint)
        {
            var credentials = new UserCredential(username, password);
            var authenticationContext = new AuthenticationContext(authenticationEndpoint + domain, 
                false);
            return await authenticationContext.AcquireTokenAsync(resource,
                  TestEnvironment.ClientIdDefault, credentials).ConfigureAwait(false);
               
        }

        private static async Task<AuthenticationResult> LoginServicePrincipalAsync(string domain, 
            string servicePrincipalId, 
            string key,
            string resource,
            string authenticationEndpoint)
        {
            var credentials = new ClientCredential(servicePrincipalId, key);
            var authenticationContext = new AuthenticationContext(authenticationEndpoint + domain, 
                false);
            return await authenticationContext.AcquireTokenAsync(resource, credentials)
                .ConfigureAwait(false);
               
        }
#if NET45

        private static async Task<AuthenticationResult> LoginWithPromptAsync(string domain,
            string resource,
            string authenticationEndpoint)
        {
            var authenticationContext = new AuthenticationContext(authenticationEndpoint + domain, 
                false, 
                TokenCache.DefaultShared);
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var task = new Task<AuthenticationResult>(() =>
            {
                var result = authenticationContext.AcquireToken(
                    resource,
                    TestEnvironment.ClientIdDefault,
                    new Uri("urn:ietf:wg:oauth:2.0:oob"),
                    PromptBehavior.Auto,
                    UserIdentifier.AnyUser);
                return result;
            });

            task.Start(scheduler);
            return await task.ConfigureAwait(false);
        }
#endif
    }
}
