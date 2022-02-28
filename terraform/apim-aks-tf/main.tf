terraform {
  required_version = ">= 0.14"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.98.0"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 2.15.0"
    }
  }
}

provider "azurerm" {
  features {

  }
}

provider "azuread" {
  client_id     = var.client_id
  client_secret = var.client_secret
  tenant_id     = var.tenant_id
}
data "azuread_client_config" "current" {}
# Retrieve domain information


