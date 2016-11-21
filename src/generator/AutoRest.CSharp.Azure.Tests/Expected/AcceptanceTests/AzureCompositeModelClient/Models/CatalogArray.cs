// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Fixtures.AcceptanceTestsAzureCompositeModelClient.Models
{
    using AcceptanceTestsAzureCompositeModelClient;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class CatalogArray
    {
        /// <summary>
        /// Initializes a new instance of the CatalogArray class.
        /// </summary>
        public CatalogArray() { }

        /// <summary>
        /// Initializes a new instance of the CatalogArray class.
        /// </summary>
        /// <param name="productArray">Array of products</param>
        public CatalogArray(IList<Product> productArray = default(IList<Product>))
        {
            ProductArray = productArray;
        }

        /// <summary>
        /// Gets or sets array of products
        /// </summary>
        [JsonProperty(PropertyName = "productArray")]
        public IList<Product> ProductArray { get; set; }

    }
}

