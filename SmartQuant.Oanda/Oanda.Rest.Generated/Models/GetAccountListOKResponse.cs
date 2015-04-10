// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// 
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using Oanda.Rest.Models;

namespace Oanda.Rest.Models
{
    public partial class GetAccountListOKResponse : IEnumerable<Account>
    {
        private IList<Account> _accounts;
        
        /// <summary>
        /// Optional.
        /// </summary>
        public IList<Account> Accounts
        {
            get { return this._accounts; }
            set { this._accounts = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the GetAccountListOKResponse class.
        /// </summary>
        public GetAccountListOKResponse()
        {
            this.Accounts = new LazyList<Account>();
        }
        
        /// <summary>
        /// Gets the sequence of accounts.
        /// </summary>
        public IEnumerator<Account> GetEnumerator()
        {
            return this.Accounts.GetEnumerator();
        }
        
        /// <summary>
        /// Gets the sequence of accounts.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// Deserialize the object
        /// </summary>
        public virtual void DeserializeJson(JToken inputObject)
        {
            if (inputObject != null && inputObject.Type != JTokenType.Null)
            {
                JToken accountsSequence = ((JToken)inputObject["accounts"]);
                if (accountsSequence != null && accountsSequence.Type != JTokenType.Null)
                {
                    foreach (JToken accountsValue in ((JArray)accountsSequence))
                    {
                        Account account = new Account();
                        account.DeserializeJson(accountsValue);
                        this.Accounts.Add(account);
                    }
                }
            }
        }
    }
}
