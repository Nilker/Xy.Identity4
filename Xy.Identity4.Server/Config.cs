using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Xy.Identity4.Server
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }
        ///// <summary>
        ///// 获取 客户端
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<Client> GetClients()
        //{
        //    return new List<Client>
        //    {
        //        new Client
        //        {
        //            ClientId = "ro.client",
        //            // 秘钥
        //            ClientSecrets ={new Secret("secret".Sha256())},

        //            ////没有用户交互，使用 clientId和 ClientSecrets 进行登录
        //            //AllowedGrantTypes = GrantTypes.ClientCredentials,

        //            //需要账号，密码 授权
        //            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,



        //            // scopes that client has access to
        //            AllowedScopes = { "api1" }
        //        }
        //    };
        //}


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // other clients omitted...

                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                }
            };
        }

        /// <summary>
        /// 获取 用户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "lhl",
                    Password = "lhl",
                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
