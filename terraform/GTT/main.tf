resource "azurerm_resource_group" "app" {
  name     = "rg-${local.resource_suffix}"
  location = var.location
  tags     = local.tags
}