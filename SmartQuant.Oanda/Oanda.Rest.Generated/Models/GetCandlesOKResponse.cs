// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using Oanda.Rest.Models;

namespace Oanda.Rest.Models
{
    public partial class GetCandlesOKResponse : IEnumerable<Candle>
    {
        private IList<Candle> _candles;
        
        /// <summary>
        /// Optional.
        /// </summary>
        public IList<Candle> Candles
        {
            get { return this._candles; }
            set { this._candles = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the GetCandlesOKResponse class.
        /// </summary>
        public GetCandlesOKResponse()
        {
            this.Candles = new LazyList<Candle>();
        }
        
        /// <summary>
        /// Gets the sequence of candles.
        /// </summary>
        public IEnumerator<Candle> GetEnumerator()
        {
            return this.Candles.GetEnumerator();
        }
        
        /// <summary>
        /// Gets the sequence of candles.
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
                JToken candlesSequence = ((JToken)inputObject["candles"]);
                if (candlesSequence != null && candlesSequence.Type != JTokenType.Null)
                {
                    foreach (JToken candlesValue in ((JArray)candlesSequence))
                    {
                        Candle candle = new Candle();
                        candle.DeserializeJson(candlesValue);
                        this.Candles.Add(candle);
                    }
                }
            }
        }
    }
}