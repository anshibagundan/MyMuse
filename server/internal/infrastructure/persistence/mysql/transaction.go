package mysql

import (
	"context"

	"gorm.io/gorm"
)

type transactionKey struct{}

type transactionClient struct {
	db *gorm.DB
}

type ITransactionClient interface {
	Run(ctx context.Context, action func(ctx context.Context) (interface{}, error)) (interface{}, error)
}

func NewTransactionClient(db *gorm.DB) ITransactionClient {
	return &transactionClient{
		db: db,
	}
}

func (r transactionClient) Run(ctx context.Context, action func(ctx context.Context) (interface{}, error)) (interface{}, error) {
	tx := r.db.Begin()

	ctx = context.WithValue(ctx, transactionKey{}, tx)

	data, err := action(ctx)

	if err != nil {
		tx.Rollback()
		return nil, err
	}
	defer func() {
		if p := recover(); p != nil {
			tx.Rollback()
		}
	}()

	tx.Commit()
	return data, nil
}
