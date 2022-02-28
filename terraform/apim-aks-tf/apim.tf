resource "azurerm_api_management" "apim" {
  name                = "logcorner-apim-aks"
  location            = azurerm_resource_group.apim-aks.location
  resource_group_name = azurerm_resource_group.apim-aks.name
  publisher_name      = "Nills"
  publisher_email     = "nilfran@microsoft.com"

  sku_name = "Developer_1"

  virtual_network_type = "External"

  virtual_network_configuration {
      subnet_id = "${azurerm_subnet.apim.id}"
  }
}

resource "azurerm_api_management_api" "back-end-api" {
  name                = "example-api"
  resource_group_name = azurerm_resource_group.apim-aks.name
  api_management_name = azurerm_api_management.apim.name
  revision            = "1"
  display_name        = "Example API"
  path                = "api"
  service_url          = "http://10.10.1.5"
  protocols = ["http"]

  import {
    content_format = "openapi-link"
    content_value  = "http://10.10.1.5/swagger/v1/swagger.json"
  }
}

resource "azurerm_api_management_product" "product" {
  product_id            = "nginx"
  api_management_name   = azurerm_api_management.apim.name
  resource_group_name   = azurerm_resource_group.apim-aks.name
  display_name          = "Test Product"
  subscription_required = false
  approval_required     = false
  published             = true
}

resource "azurerm_api_management_product_api" "example" {
  api_name            = azurerm_api_management_api.back-end-api.name
  product_id          = azurerm_api_management_product.product.product_id
  api_management_name = azurerm_api_management.apim.name
  resource_group_name = azurerm_api_management.apim.resource_group_name
}