using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saltus.digiTICKET.DataEnums;
using Saltus.digiTICKET.DataInterfaces;
using SharedScriptsApi.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Saltus.digiTICKET.Utilities.Helpers
{
    public class DateTimeHelper
    {

        /// <summary>
        /// Converts DateTimes in a JToken to the appropriate date value for both request and response
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="appConfigProvider"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static JToken ConvertDates(JToken value, HttpType type)
        {
            var retVal = value; 

            if (retVal.GetType() == typeof(JArray))
            {
                List<JToken> temp = new List<JToken>();
                retVal.ToObject<JArray>();
                foreach (var token in value)
                {
                    retVal = ProcessJTokenDate(token, type);
                    if (retVal == null)
                    {
                        throw new InvalidOperationException();
                    }
                    temp.Add(token);
                }
                retVal = new JArray(temp);
            }
            else
            {
                retVal = ProcessJTokenDate(value, type);
                if (retVal == null)
                {
                    throw new InvalidOperationException();
                }
            }

            return retVal;
        }


        /// <summary>
        /// Grabs DateTime tokens and sets the to the values for a request or response (recursive)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <param name="appConfigProvider"></param>
        /// <returns></returns>
        private static JToken ProcessJTokenDate(JToken token, HttpType type)
        {
            JToken dateToken = null;

            JObject jObject = token.ToObject<JObject>();

            foreach (KeyValuePair<string, JToken> property in jObject)
            {
                // if the property is an object itself go through and convert all the DateTimes
                if (property.Value.GetType() == typeof(JObject))
                {
                    jObject[property.Key] = ConvertDates(jObject[property.Key], type);
                }
                else
                {
                    
                    if (jObject.TryGetValue(property.Key, StringComparison.OrdinalIgnoreCase, out JToken value))
                    {
                        // check if the JValue JToken is a Date
                        if (property.Value != null && property.Value.Type != JTokenType.TimeSpan && property.Value.Type == JTokenType.Date)
                        {
                            // makes sure that even though the type is a Date it can be converted to a DateTime
                            if (jObject.TryGetProperty(property.Key, out DateTime dateTime))
                            {
                                if (type.Equals(HttpType.Request))
                                {
                                    dateTime = property.Key.Equals("modifiedDate", StringComparison.OrdinalIgnoreCase) ? dateTime.ConvertToTimezone("Central Standard Time") : dateTime.ConvertToTimezone();
                                }
                                else
                                {
                                    dateTime = dateTime.ToUniversalTime();
                                }

                                if (JToken.FromObject(dateTime) != null)
                                {

                                    jObject[property.Key] = JToken.FromObject(dateTime);
                                }
                            }
                        }
                    }
                }
            }

            return jObject;
        }
    }
}
