from node:latest as build

workdir /app
copy package.json /app/
run npm install
copy ./ /app/
run npm run build

from nginx:latest as run

expose 80
copy --from=build /app/dist /usr/share/nginx/html
copy ./nginx.conf /etc/nginx/conf.d/default.conf

