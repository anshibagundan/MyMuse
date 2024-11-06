FROM golang:1.23-alpine

# サブディレクトリをワーキングディレクトリとして設定
WORKDIR /app/server

COPY . .

RUN go mod download

# 本番用にビルド
RUN CGO_ENABLED=0 GOOS=linux go build -o main ./cmd/api/main.go

# 実行
CMD ["/app/server/main"]