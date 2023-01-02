# Parse the config file
$configPath = "$PSScriptRoot\..\Configuration.json"
if (-Not $(Test-Path $configPath))
{
    throw "You need to create $configPath."
}
$configContent = Get-Content $configPath | Join-String
$config  = ConvertFrom-Json $configContent

dotnet user-secrets set "blobContainerUri" "$($config.BlobContainerUri)" --id "55b7e1c9-a359-4297-96f1-1bb82d3727d1"

# Check if we need to authenticate
$subs = Get-AzSubscription
if (-not $?)
{
    Connect-AzAccount
}

# Get the secret from Key Vault
$clientSecret = Get-AzKeyVaultSecret -VaultName "$($config.KeyVaultName)" -SecretName "ClientSecret" -AsPlainText

# Store them to the Creds.env file for use in the container
$envContent =
"AZURE_TENANT_ID=$($config.TenantId)
AZURE_CLIENT_ID=$($config.ClientId)
AZURE_CLIENT_SECRET=$clientSecret"
Set-Content -Path $(Join-Path $PSScriptRoot "Creds.env") -Value $envContent