from django.contrib.auth.models import User
from rest_framework import serializers

from .models import Photo, Tag


class PhotoSerializer(serializers.ModelSerializer):
    class Meta:
        model = Photo
        fields = ['id', 'title', 'detailed_title', 'user', 'time', 'photo_num', 'content', 'height', 'width', 'tag']
        
class UserSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ['id', 'username', 'password']

class TagSerializer(serializers.ModelSerializer):
    class Meta:
        model = Tag
        fields = ['id', 'tag', 'user']
