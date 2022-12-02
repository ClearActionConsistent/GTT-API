locals {
  resource_suffix = "${var.name}-${terraform.workspace}-${var.location}-${var.suffix}"
  environment     = terraform.workspace
  tags = merge(var.tags, {
    "environment" = terraform.workspace,
    "app"         = var.name
  })
}

variable "name" {
  default     = "gtt"
  description = "The name of the resource. (Example: gtt)"
  nullable    = false
}

variable "location" {
  default     = "eastasia"
  description = "The Azure location where all resources should be created."
  nullable    = false
}

variable "suffix" {
  default     = "0001"
  description = "Suffix for resources per naming conventions for immutability."
}

variable "tags" {
  type    = map(string)
  default = {}
}