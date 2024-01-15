FROM node:20.10.0-alpine AS base

WORKDIR /app

COPY ./angular .

RUN npm i -g @angular/cli

RUN npm i

ENTRYPOINT ["ng", "serve", "--host", "0.0.0.0"]
