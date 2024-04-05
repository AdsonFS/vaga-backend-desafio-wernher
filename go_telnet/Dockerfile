FROM golang:1.22

WORKDIR /app

COPY go.mod main.go ./
RUN CGO_ENABLED=0 GOOS=linux go build -o /app/telnet-server

# telnet port
EXPOSE 23
ENTRYPOINT [ "./telnet-server" ]