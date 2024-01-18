FROM node:20.10.0-alpine AS build
WORKDIR /app
COPY . .
RUN npm i
RUN npx ng build --aot

FROM nginx:stable-alpine
COPY nginx/ /etc/nginx/
COPY --from=build /app/dist/tg-drive /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
