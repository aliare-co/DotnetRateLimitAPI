Criar a imagem Docker do projeto, já com a tag `latest`.
`docker build -t tetri/dotnet-rate-limit:latest -f Dockerfile .`

Publicar a imagem Docker no registry. Lembre-se de autenticar antes utilizando o comando `docker login`.
`docker push tetri/dotnet-rate-limit:latest`

Executar o deployment no k8s.
`kubectl apply -f deploy.yml`

Executar o service no k8s.
`kubectl apply -f service.yml`

Após alterações no projeto, refaça todos os passos anteriores e reinicie o deployment.
`kubectl rollout restart deployment/dotnet-rate-limit-deployment`
