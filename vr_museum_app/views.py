import io
import logging
from io import BytesIO

import requests
from django.conf import settings
from django.contrib import messages
from django.contrib.auth import authenticate, login
from django.contrib.auth.decorators import login_required
from django.contrib.auth.models import User
from django.core.files.uploadedfile import InMemoryUploadedFile
from django.shortcuts import redirect, render
from django.views import generic
from PIL import Image
from rest_framework import permissions, viewsets
from rest_framework.response import Response
from rest_framework.views import APIView

from .forms import PhotoForm, TagForm, TagForm_delete
from .models import Photo, Tag
from .serializers import PhotoSerializer, UserSerializer

logger = logging.getLogger(__name__)

class PhotoViewSet(viewsets.ModelViewSet):
    queryset = Photo.objects.all()
    serializer_class = PhotoSerializer

def user_login(request):
    if request.method == 'POST':
        username = request.POST.get('username')
        password = request.POST.get('password')

        # ユーザー認証
        user = authenticate(request, username=username, password=password)

        if user is not None:
            # ログイン成功
            login(request, user)
            return redirect('title')
        else:
            # ログイン失敗
            messages.error(request, 'ユーザー名またはパスワードが間違っています。')
            return render(request, 'login.html')
        
    return render(request, 'login.html' ,{'MEDIA_URL':settings.MEDIA_URL})

def user_create(request):
    if request.method == 'POST':
        new_username = request.POST.get('new_username')
        new_password = request.POST.get('new_password')

        try:
            # 新しいユーザーオブジェクトを作成し、ユーザー名とハッシュ化されたパスワードを設定
            user = User.objects.create_user(username=new_username, password=new_password)
            # ログメッセージ作成
            logger.info('ユーザーの作成に成功しました。ユーザー名: %s', new_username)
            Tag.objects.create(user=user, tag='s1')
        except Exception as e:
            # ログメッセージ作成
            logger.error('ユーザーの作成に失敗しました。エラー: %s', e)
            messages.error(request, 'ユーザーの作成に失敗しました。エラー: {}'.format(str(e)))

        # ログイン処理
        user = authenticate(request, username=new_username, password=new_password)  # ハッシュ化されたパスワードを使用する
        if user is not None:
            messages.error(request, 'ログインに成功しました。')
            login(request, user)
            return redirect('title')
        else:
            messages.error(request, 'ユーザー名またはパスワードが間違っています。ユーザー名: {}'.format(new_password))


    return render(request, 'login_create.html', {'MEDIA_URL':settings.MEDIA_URL})


from django.db import transaction


@login_required
def index(request):
    obj = Photo.objects.all()
    if request.method == 'POST':
        form = PhotoForm(request.POST, request.FILES, user=request.user)
        tag_form = TagForm(request.POST, user=request.user)
        tag_delete_form = TagForm_delete(request.POST, user=request.user)
        if form.is_valid():
            with transaction.atomic():  # Start a transaction
                photo = form.save(commit=False)
                photo.user = request.user

                image_file = request.FILES['content']
                image = Image.open(io.BytesIO(image_file.read()))
                photo.width, photo.height = image.size
                photo.tag = request.POST.get('tag')
                new_photo_num = int(request.POST.get('photo_num'))
                max_photo_num = Photo.objects.count() + 1

                if new_photo_num > max_photo_num:
                    photo.photo_num = max_photo_num
                else:
                    # Shift existing photo numbers
                    qs = Photo.objects.filter(photo_num__gte=new_photo_num).order_by('-photo_num')
                    for p in qs:
                        p.photo_num += 1
                        p.save()
                    photo.photo_num = new_photo_num

                photo.save()
            return redirect('title')
        
        elif tag_form.is_valid():
            tag = tag_form.save(commit=False)
            tag.user = request.user
            tag.save()
            return redirect('title')
        
        elif tag_delete_form.is_valid():
                selected_tag = tag_delete_form.cleaned_data['tag']
                if not Photo.objects.filter(tag=selected_tag).exists():
                    selected_tag.delete()
                    messages.success(request, 'タグが削除されました。')
                    return redirect('title')
                else:
                    alert_message = f'写真がタグ "{selected_tag}" に関連付けられているため、削除できませんでした。'
                    messages.warning(request, alert_message)
                    return render(request, 'index.html', {'alert_message': alert_message})

    else:
        form = PhotoForm(user=request.user)
        tag_form = TagForm(user=request.user)
        tag_delete_form = TagForm_delete(user=request.user)
        obj = Photo.objects.all().order_by('photo_num')
    return render(request, 'index.html', {'form': form, 'tag_form' : tag_form, 'tag_delete_form' : tag_delete_form, 'obj': obj, 'MEDIA_URL': settings.MEDIA_URL})



class PhotoModelListView(APIView):
    serializer_class = PhotoSerializer
    permission_classes = [permissions.AllowAny]

    def get_queryset(self):
        return Photo.objects.all().order_by('photo_num')

    def get(self, request):
        queryset = self.get_queryset()  # get_queryset() メソッドを呼び出してクエリセットを取得
        serializer = self.serializer_class(queryset, many=True)
        return Response(serializer.data)
    

class UserModelListView(APIView):
    serializer_class = UserSerializer
    permission_classes = [permissions.AllowAny]

    def get_queryset(self):
        return User.objects.all()

    def get(self, request):
        queryset = self.get_queryset()  # get_queryset() メソッドを呼び出してクエリセットを取得
        serializer = self.serializer_class(queryset, many=True)
        return Response(serializer.data)



