# Stage 1: Compile and Build angular codebase
FROM node:latest as build
WORKDIR /usr/local/app
COPY ./OrchesterApp.Frontend /usr/local/app/
RUN npm install
RUN npm run build --prod

# Stage 2: Serve app with nginx server
FROM nginx:latest
COPY ./OrchesterApp.Frontend/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /usr/local/app/www /usr/share/nginx/html
