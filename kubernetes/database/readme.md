kubectl apply -f .

kubectl delete -f .

kubectl get pods  

kubectl port-forward svc/workshop-todolist-webapi-service 8080:80

localhost:8080/swagger/index.html