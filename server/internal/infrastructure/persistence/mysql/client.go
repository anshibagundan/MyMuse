package mysql

import (
	"context"

	"gorm.io/gorm"
)

type MysqlClient interface {
	WithContext(ctx context.Context) *gorm.DB
}

type mysqlClient struct {
	db *gorm.DB
}

func NewMysqlClient(db *gorm.DB) MysqlClient {
	return &mysqlClient{
		db: db,
	}
}

func (r *mysqlClient) WithContext(ctx context.Context) *gorm.DB {
	if tx := ctx.Value(transactionKey{}); tx != nil {
		return tx.(*gorm.DB).WithContext(ctx)
	}
	return r.db.WithContext(ctx)
}
