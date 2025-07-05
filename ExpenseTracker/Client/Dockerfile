FROM node:22-alpine AS builder

WORKDIR /app

COPY package*.json ./

RUN npm install --legacy-peer-deps

COPY . .

RUN npm run build -- --configuration production

FROM nginx:alpine AS production

RUN rm -rf /usr/share/nginx/html/*

COPY --from=builder /app/dist/Client/browser /usr/share/nginx/html

# Force rebuild
COPY nginx.conf /etc/nginx/conf.d/default.conf