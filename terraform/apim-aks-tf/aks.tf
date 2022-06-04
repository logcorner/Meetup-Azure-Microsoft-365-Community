
resource "azurerm_kubernetes_cluster" "test" {
  name                = "aks-for-apim"
  location            = azurerm_resource_group.apim-aks.location
  resource_group_name = azurerm_resource_group.apim-aks.name
  dns_prefix          = "nfaksapim"
  default_node_pool {
    name       = "default"
    node_count = 1
    vm_size    = "Standard_D2_v2"

    vnet_subnet_id = azurerm_subnet.aks.id
  }

  service_principal {
    client_id     = var.client_id
    client_secret = var.client_secret
  }

}
output "kube_config" {
  value     = azurerm_kubernetes_cluster.test.kube_config_raw
  sensitive = true
}