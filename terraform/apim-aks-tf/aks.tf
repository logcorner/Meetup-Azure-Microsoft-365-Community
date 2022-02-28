

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
    client_id     = "5c4919f0-7d40-40ec-837c-8a6a73c47ed3"  //azuread_application.aksapim.application_id
    client_secret = "G2s7Q~c3MfgrpqqgrT_sXHQkJMY2dv7fIBVF4" //random_string.sp-password.result
  }

}

# resource "azurerm_role_assignment" "test" {
#   scope                = azurerm_resource_group.apim-aks.id
#   role_definition_name = "Contributor"
#   principal_id         = "5c4919f0-7d40-40ec-837c-8a6a73c47ed3" //azuread_service_principal.aksapim.id
# }

output "client_certificate" {
  value = azurerm_kubernetes_cluster.test.kube_config.0.client_certificate
}

output "kube_config" {
  value     = azurerm_kubernetes_cluster.test.kube_config_raw
  sensitive = true
}