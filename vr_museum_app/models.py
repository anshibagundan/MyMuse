from django.db import models


class Photo(models.Model):
    title = models.CharField(max_length=40)
    detailed_title = models.CharField(max_length=100)
    user = models.CharField(max_length=20)
    time = models.DateField()
    content = models.ImageField(upload_to='documents/', default='defo')
    height = models.IntegerField()
    width = models.IntegerField()
    tag = models.CharField(max_length=20)
    photo_num = models.IntegerField(default=0)

    def __str__(self):
        return self.title
    
class Tag(models.Model):
    tag = models.CharField(max_length=20)
    user = models.CharField(max_length=20)
    
    def __str__(self):
        return self.tag