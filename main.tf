provider "azurerm" {
    features {
      
    }
  
}

resource "azurerm_resource_group" "rg" {
  name = var.resourse_group_name
  location = var.location
}

resource "azurerm_service_plan" "serviceplan" {
  name = "gunjanserviceplan"
  location = var.location
  resource_group_name = var.resourse_group_name
  os_type = "Linux"
  sku_name = "S1"
}

resource "azurerm_linux_web_app" "webapp" {
  name = "terraformwebapp"
  resource_group_name = var.resourse_group_name
  location = var.location
  service_plan_id = azurerm_service_plan.serviceplan.id

  app_settings = {
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
  }
  site_config {
      application_stack {
      docker_image_name   = "gunjangoyal1911044/mywebapi:latest"
      docker_registry_url = "https://index.docker.io/"
  }
}
}

resource "azurerm_mssql_server" "sqlserver" {
  name = "gunjansqlserver"
  resource_group_name = var.resourse_group_name
  location = var.location
  version = "12.0"
  administrator_login = "admin123"
  administrator_login_password = "Password@1122"  
}

resource "azurerm_mssql_database" "sqldb" {
  name = "bookdb123"
  server_id = azurerm_mssql_server.sqlserver.id
  sku_name = "Basic"
  max_size_gb    = 2
  zone_redundant = false
  tags = {
    foo = "bar"
  }
}