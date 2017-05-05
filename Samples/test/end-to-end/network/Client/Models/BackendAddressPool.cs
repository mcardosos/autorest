// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ApplicationGateway.Models
{
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Pool of backend IP addresses.
    /// </summary>
    [JsonTransformation]
    public partial class BackendAddressPool : SubResource
    {
        /// <summary>
        /// Initializes a new instance of the BackendAddressPool class.
        /// </summary>
        public BackendAddressPool()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BackendAddressPool class.
        /// </summary>
        /// <param name="id">Resource ID.</param>
        /// <param name="backendIPConfigurations">Gets collection of references
        /// to IP addresses defined in network interfaces.</param>
        /// <param name="loadBalancingRules">Gets load balancing rules that use
        /// this backend address pool.</param>
        /// <param name="outboundNatRule">Gets outbound rules that use this
        /// backend address pool.</param>
        /// <param name="provisioningState">Get provisioning state of the
        /// public IP resource. Possible values are: 'Updating', 'Deleting',
        /// and 'Failed'.</param>
        /// <param name="name">Gets name of the resource that is unique within
        /// a resource group. This name can be used to access the
        /// resource.</param>
        /// <param name="etag">A unique read-only string that changes whenever
        /// the resource is updated.</param>
        public BackendAddressPool(string id = default(string), IList<NetworkInterfaceIPConfiguration> backendIPConfigurations = default(IList<NetworkInterfaceIPConfiguration>), IList<SubResource> loadBalancingRules = default(IList<SubResource>), SubResource outboundNatRule = default(SubResource), string provisioningState = default(string), string name = default(string), string etag = default(string))
            : base(id)
        {
            BackendIPConfigurations = backendIPConfigurations;
            LoadBalancingRules = loadBalancingRules;
            OutboundNatRule = outboundNatRule;
            ProvisioningState = provisioningState;
            Name = name;
            Etag = etag;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets collection of references to IP addresses defined in network
        /// interfaces.
        /// </summary>
        [JsonProperty(PropertyName = "properties.backendIPConfigurations")]
        public IList<NetworkInterfaceIPConfiguration> BackendIPConfigurations { get; private set; }

        /// <summary>
        /// Gets load balancing rules that use this backend address pool.
        /// </summary>
        [JsonProperty(PropertyName = "properties.loadBalancingRules")]
        public IList<SubResource> LoadBalancingRules { get; private set; }

        /// <summary>
        /// Gets outbound rules that use this backend address pool.
        /// </summary>
        [JsonProperty(PropertyName = "properties.outboundNatRule")]
        public SubResource OutboundNatRule { get; private set; }

        /// <summary>
        /// Gets or sets get provisioning state of the public IP resource.
        /// Possible values are: 'Updating', 'Deleting', and 'Failed'.
        /// </summary>
        [JsonProperty(PropertyName = "properties.provisioningState")]
        public string ProvisioningState { get; set; }

        /// <summary>
        /// Gets name of the resource that is unique within a resource group.
        /// This name can be used to access the resource.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a unique read-only string that changes whenever the
        /// resource is updated.
        /// </summary>
        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; set; }

    }
}
