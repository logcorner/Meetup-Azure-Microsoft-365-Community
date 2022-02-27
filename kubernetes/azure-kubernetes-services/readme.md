$resourceGroup ='Meetup-Azure-Microsoft-365-Community'
$location ='westeurope'
$clusterName ='democluster'
az group create --name $resourceGroup --location $location

az aks create --resource-group $resourceGroup --name $clusterName --location $location --generate-ssh-keys

az aks install-cli

az aks get-credentials --resource-group $resourceGroup --name $clusterName

kubectl delete -f database
kubectl apply -f database
kubectl get pods 
kubectl apply -f webapi

kubectl delete -f .

kubectl get pods  

kubectl port-forward svc/workshop-todolist-webapi-service 8080:80

localhost:8080/swagger/index.html


docker login 