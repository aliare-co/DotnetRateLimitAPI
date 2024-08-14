docker build -t tetri/dotnet-rate-limit:latest -f Dockerfile .

docker push tetri/dotnet-rate-limit:latest

kubectl apply -f deploy.yml
kubectl apply -f service.yml
-ou-
kubectl rollout restart deployment/dotnet-rate-limit-deployment
