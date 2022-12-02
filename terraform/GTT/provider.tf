terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.34.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "rg-terraform-state-shared"
    storage_account_name = "stterraformstategttapi"
    container_name       = "tfstate"
    key                  = "gttapi.tfstate"
  }
}

provider "azurerm" {
  #subscription_id = "qwerqwer"
  #tenant_id = "wrqwerqw"
  #client_id = "werqwer"
  #client_secret = "t0fwerqwerwer"
  features { # no need any features for now
  }
}

