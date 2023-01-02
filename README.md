# EnvironmentCredentialExample
An example for using an [`EnvironmentCredential`](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.environmentcredential?view=azure-dotnet) to connect to Azure from a container.

## Dependencies
- [Power Shell 7.3](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.3)
- [The Az PowerShell module](https://learn.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-9.2.0)
- A Password-based Service Principal [PowerShell steps](https://learn.microsoft.com/en-us/powershell/azure/create-azure-service-principal-azureps?view=azps-9.2.0)
- A Key Vault with the secret `ClientSecret` for the Service Principal's client secret.
- A Blob container where the Service Principal has been assigned the "Storage Blob Data Reader" role.

## Getting started
The Blob, KeyVault, and identifiers for the Service Principal used are configured using `Configuration.json` at the root of the repo it should look like:
```json
{
  "BlobContainerUri": "https://<StorageAccount>.blob.core.windows.net/<ContainerName>",
  "KeyVaultName": "<VaultNameHere>",
  "TenantId": "<The Service Principal's Tenant's Id>",
  "ClientId": "<The Service Principal's AppId>"
}
```
> **Note**
> This file is just a convenience to make this example reusable when consumers don't all have access to shared resources.

To make the script for getting the environment variables not require authentication each time you can run:
```powershell
Connect-AzAccount -Scope CurrentUser
```
> **Warning**
> If you don't run the above command the script Create-EnvFile.ps1 will open a browser to authenticate with Azure when run, which can happen automatically when the solution opens.

With the above configured you can open the solution and F5 to see an example of authenticating to Azure using the EnvironmentCredential within a container. If everything is configured correctly the Debug pane in the output window will have a list of the blobs in the container.

## Highlights
### [Create-EnvFile.ps1](EnvironmentCredentialExample/Create-EnvFile.ps1)
Accesses the Key Vault in Azure to get the Service Principal's client secret and creates Creds.env with the needed environment variables for using `EnvironmentCredential`.
### CreateEnv target in [EnvironmentCredentialExample\EnvironmentCredentialExample.csproj](EnvironmentCredentialExample/EnvironmentCredentialExample.csproj)
This target handles running Create-EnvFile.ps1 when the project is opened or built in VS (so you don't have to remember to run the script before opening the project)
### [`DockerfileRunEnvironmentFiles`](https://learn.microsoft.com/en-us/visualstudio/containers/container-msbuild-properties) property in [EnvironmentCredentialExample\EnvironmentCredentialExample.csproj](EnvironmentCredentialExample/EnvironmentCredentialExample.csproj)
Configures the container tools to use the generated Creds.env when creating the container for debugging
### [docker-compose.vs.debug.yml](docker-compose.vs.debug.yml)
Configures the docker-compose project to use Creds.env for the `environmentcredentialexample` service when debugging [Docs](https://docs.microsoft.com/en-us/visualstudio/containers/docker-compose-properties?view=vs-2022#overriding-visual-studios-docker-compose-configuration)
