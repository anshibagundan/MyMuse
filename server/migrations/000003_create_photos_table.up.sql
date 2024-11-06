CREATE TABLE photos (
                        id INT PRIMARY KEY AUTO_INCREMENT,
                        title VARCHAR(20) NOT NULL,
                        detailed VARCHAR(100) NOT NULL,
                        content TEXT NOT NULL,
                        height INT NOT NULL,
                        width INT NOT NULL,
                        tag_id INT NOT NULL,  -- tagsテーブルのIDとリレーション
                        photo_order INT NOT NULL,  -- タグ内での写真の順番
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                        updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        FOREIGN KEY (tag_id) REFERENCES tags(id) ON DELETE CASCADE
);
