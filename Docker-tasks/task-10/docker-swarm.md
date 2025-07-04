docker service create \
  --name=viz \
  --publish=8081:8080 \
  --constraint=node.role==manager \
  --mount=type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
  dockersamples/visualizer


  ![alt text](image.png)