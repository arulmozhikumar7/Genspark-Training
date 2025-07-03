FROM node:20-alpine as builder

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build --configuration production

# Stage 2: Serve with NGINX
FROM nginx:1.25-alpine

# Copy the build output from previous stage
COPY --from=builder /app/dist/Client /usr/share/nginx/html


# Expose port 80
EXPOSE 80

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]