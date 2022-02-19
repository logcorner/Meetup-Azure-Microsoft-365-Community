# install azure cli
https://docs.microsoft.com/fr-fr/cli/azure/install-azure-cli-windows?tabs=azure-cli

az --version
azure-cli                         2.31.0 *

# Installer et configurer kubectl

https://kubernetes.io/fr/docs/tasks/tools/install-kubectl/
https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/

kubectl version --client

# build and tag images 
docker-compoe build 

# prepare and push images to azure container registry
az login
az account set --subscription YOUR_SUBSCRIPTION_ID
az acr login --name aksacrkhhdlog
docker tag logcornerhub/todo-list-mssql-tools  aksacrkhhdlog.azurecr.io/todo-list-mssql-tools
docker push aksacrkhhdlog.azurecr.io/todo-list-mssql-tools

docker tag logcornerhub/todo-list-web-api   aksacrkhhdlog.azurecr.io/todo-list-web-api
docker push aksacrkhhdlog.azurecr.io/todo-list-web-api

# prepare and deploy to aks

az aks get-credentials --resource-group $resourceGroupName --name $clusterName

C:\Users\tocan\.kube\config

az aks browse --resource-group=$resourceGroupName --name=$clusterName

# kubernetes dashboad
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/master/aio/deploy/recommended.yaml
kubectl proxy

http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/login

kubectl -n kubernetes-dashboard get secret $(kubectl -n kubernetes-dashboard get sa/admin-user -o jsonpath="{.secrets[0].name}") -o go-template="{{.data.token | base64decode}}"

kubectl create namespace aks

kubectl apply -f . -f Database  -f WebApi
kubectl rollout restart deployment speech-command-http-api-deployment -n aks


kubectl get pods -n aks
cls# enable managed identity on azure container registry
kubectl create secret docker-registry registrysecret --docker-server=aksacrkhhdlog.azurecr.io --docker-username=aksacrkhhdlog --docker-password=FLSseMqJB5bU+pOEOnngMMxNc9tuSbVR --docker-email=testaks@yahoo.fr -n aks
# add acrpull role to aks