terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "3.23.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "IWA-rg-Dev" {
  name     = "iwa-rg-pgsql"
  location = "Canada Central"
}

resource "azurerm_postgresql_server" "IWA-rg-pg-server" {
  name = "iwa-postgresql-server-1"
  location = azurerm_resource_group.IWA-rg-Dev.location
  resource_group_name = azurerm_resource_group.IWA-rg-Dev.name

  sku_name = "GP_Gen5_8"

  storage_mb = 640000
  
  backup_retention_days        = 7
  geo_redundant_backup_enabled = false
  auto_grow_enabled            = true

  administrator_login          = "iwaadmin"
  administrator_login_password = "novell@123"
  version                      = "9.6"

  public_network_access_enabled    = true
  ssl_enforcement_enabled      = false
  ssl_minimal_tls_version_enforced = "TLSEnforcementDisabled"

  tags = {
    "Owner" = "IWA_Dev",
    "Environment" = "Development",
    "Terraform" = "True"
    }

  }

resource "azurerm_postgresql_firewall_rule" "IWA-rg-pg-firewall" {
  name                = "access-from-desktop"
  resource_group_name = azurerm_resource_group.IWA-rg-Dev.name
  server_name         = azurerm_postgresql_server.IWA-rg-pg-server.name
  start_ip_address    = "49.36.144.35"
  end_ip_address      = "49.36.144.35"
}

resource "azurerm_postgresql_database" "IWA-rg-DevDB" {
  name                = "iwa-dev"
  resource_group_name = azurerm_resource_group.IWA-rg-Dev.name
  server_name         = azurerm_postgresql_server.IWA-rg-pg-server.name
  charset             = "UTF8"
  collation           = "English_United States.1252"
}

