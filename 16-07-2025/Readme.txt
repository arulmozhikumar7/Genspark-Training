# 1. Create the Azure Function App
az functionapp create \
  --resource-group arul \
  --consumption-plan-location eastus \
  --runtime dotnet-isolated \
  --functions-version 4 \
  --name aruldotnetfunction \
  --storage-account arulblob

# 2. Set application settings
az functionapp config appsettings set \
  --name aruldotnetfunction \
  --resource-group arul \
  --settings \
    AzureStorageConnectionString="DefaultEndpointsProtocol=https;AccountName=arulblob;AccountKey=...;EndpointSuffix=core.windows.net" \
    ContainerName="demo" \
    KeyVaultUri="https://arul-kv.vault.azure.net/"

# 3. Enable Managed Identity for the Function App
az functionapp identity assign \
  --name aruldotnetfunction \
  --resource-group arul

# 4. Get the function key for HTTP access
az functionapp function keys list \
  --resource-group arul \
  --name aruldotnetfunction \
  --function-name Function

# 5. Grant the Function App access to Key Vault
az keyvault set-policy \
  --name arul-kv \
  --object-id 965d6d85-bef4-49a3-af6a-a511c79b9a51 \
  --secret-permissions get set

# 6. Publish the function app 
func azure functionapp publish aruldotnetfunction

# 7. Allow SAS-based blob access (does NOT make it public)
az storage account update \
  --name arulblob \
  --resource-group arul \
  --allow-blob-public-access true

# 8. Call the function with a valid blob name and function key
curl "https://aruldotnetfunction.azurewebsites.net/api/generate-sas/Demo.png?code=<function-key>"
