# ingress controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.1.1/deploy/static/provider/aws/deploy.yaml

kubectl get pods -n ingress-nginx --watch

eneable ssl
# download and install openssl
https://slproweb.com/products/Win32OpenSSL.html

openssl req -x509 -nodes -days 365 -newkey rsa:2048 -out logcorner-ingress-tls.crt -keyout logcorner-ingress-tls.key -subj "/CN=kubernetes.docker.com/O=logcorner-ingress-tls"

kubectl create secret tls logcorner-ingress-tls  --key logcorner-ingress-tls.key --cert logcorner-ingress-tls.crt

kubectl delete -f database

kubectl delete -f webapi

kubectl apply -f database

kubectl apply -f webapi

kubectl get pods  

kubectl port-forward svc/workshop-todolist-webapi-service 8080:80

http://kubernetes.docker.com/http-api/swagger/index.html

docker-compose build --no-cache

docker tag logcornerhub/todo-list-web-api logcornerhub/todo-list-web-api:ingress

docker tag logcornerhub/todo-list.mssql-server logcornerhub/todo-list.mssql-server:ingress


docker login 

docker  push logcornerhub/todo-list-web-api:ingress
docker push logcornerhub/todo-list.mssql-server:ingress

kubectl rollout restart deployment workshop-todolist-webapi-deployment