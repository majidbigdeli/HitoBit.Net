using HitoBit.Net.Clients;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HitoBit.Net
{
    /// <summary>
    /// Helper methods for the HitoBit API
    /// </summary>
    public static class HitoBitHelpers
    {
        /// <summary>
        /// Get the used weight from the response headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static int? UsedWeight(this IEnumerable<KeyValuePair<string, IEnumerable<string>>>? headers)
        {
            if (headers == null)
                return null;

            var headerValues = headers.SingleOrDefault(s => s.Key.StartsWith("X-MBX-USED-WEIGHT-", StringComparison.InvariantCultureIgnoreCase)).Value;
            if (headerValues != null && int.TryParse(headerValues.First(), out var value))
                return value;
            return null;
        }

        /// <summary>
        /// Get the used weight from the response headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static int? UsedOrderCount(this IEnumerable<KeyValuePair<string, IEnumerable<string>>>? headers)
        {
            if (headers == null)
                return null;

            var headerValues = headers.SingleOrDefault(s => s.Key.StartsWith("X-MBX-ORDER-COUNT-", StringComparison.InvariantCultureIgnoreCase)).Value;
            if (headerValues != null && int.TryParse(headerValues.First(), out var value))
                return value;
            return null;
        }

        /// <summary>
        /// Clamp a quantity between a min and max quantity and floor to the closest step
        /// </summary>
        /// <param name="minQuantity"></param>
        /// <param name="maxQuantity"></param>
        /// <param name="stepSize"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public static decimal ClampQuantity(decimal minQuantity, decimal maxQuantity, decimal stepSize, decimal quantity)
        {
            quantity = Math.Min(maxQuantity, quantity);
            quantity = Math.Max(minQuantity, quantity);
            if (stepSize == 0)
                return quantity;
            quantity -= quantity % stepSize;
            quantity = Floor(quantity);
            return quantity;
        }

        /// <summary>
        /// Clamp a price between a min and max price
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static decimal ClampPrice(decimal minPrice, decimal maxPrice, decimal price)
        {
            price = Math.Min(maxPrice, price);
            price = Math.Max(minPrice, price);
            return price;
        }

        /// <summary>
        /// Floor a price to the closest tick
        /// </summary>
        /// <param name="tickSize"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static decimal FloorPrice(decimal tickSize, decimal price)
        {
            price -= price % tickSize;
            price = Floor(price);
            return price;
        }

        /// <summary>
        /// Floor
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal Floor(decimal number)
        {
            return Math.Floor(number * 100000000) / 100000000;
        }

        /// <summary>
        /// Add the IHitoBitClient and IHitoBitSocketClient to the sevice collection so they can be injected
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="defaultOptionsCallback">Set default options for the client</param>
        /// <param name="socketClientLifeTime">The lifetime of the IHitoBitSocketClient for the service collection. Defaults to Scoped.</param>
        /// <returns></returns>
        public static IServiceCollection AddHitoBit(
            this IServiceCollection services, 
            Action<HitoBitClientOptions, HitoBitSocketClientOptions>? defaultOptionsCallback = null,
            ServiceLifetime? socketClientLifeTime = null)
        {
            if (defaultOptionsCallback != null)
            {
                var options = new HitoBitClientOptions();
                var socketOptions = new HitoBitSocketClientOptions();
                defaultOptionsCallback?.Invoke(options, socketOptions);

                HitoBitClient.SetDefaultOptions(options);
                HitoBitSocketClient.SetDefaultOptions(socketOptions);
            }

            services.AddTransient<IHitoBitClient, HitoBitClient>();
            if (socketClientLifeTime == null)
                services.AddScoped<IHitoBitSocketClient, HitoBitSocketClient>();
            else
                services.Add(new ServiceDescriptor(typeof(IHitoBitSocketClient), typeof(HitoBitSocketClient), socketClientLifeTime.Value));
            return services;
        }

        /// <summary>
        /// Validate the string is a valid HitoBit symbol.
        /// </summary>
        /// <param name="symbolString">string to validate</param>
        public static void ValidateHitoBitSymbol(this string symbolString)
        {
            if (string.IsNullOrEmpty(symbolString))
                throw new ArgumentException("Symbol is not provided");

            if(!Regex.IsMatch(symbolString, "^([A-Z|a-z|0-9]{5,})$"))
                throw new ArgumentException($"{symbolString} is not a valid HitoBit symbol. Should be [BaseAsset][QuoteAsset], e.g. BTCUSDT");
        }
    }
}
