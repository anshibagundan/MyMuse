import io
import json
import logging

from django.contrib.auth.mixins import LoginRequiredMixin, UserPassesTestMixin
from django.contrib.auth import get_user_model
from django.http import JsonResponse
from django.conf import settings
from django.contrib import messages
from django.contrib.auth import authenticate, login
from django.contrib.auth.decorators import login_required
from django.contrib.auth.models import User
from django.core.files.uploadedfile import InMemoryUploadedFile
from django.shortcuts import redirect, render, get_object_or_404
from django.urls import reverse_lazy
from django.views import generic
from PIL import Image
from rest_framework import permissions, viewsets
from rest_framework.response import Response
from rest_framework.views import APIView

from .forms import PhotoForm, TagForm
from .models import Photo, Tag
from .serializers import PhotoSerializer, UserSerializer, TagSerializer
from django.db import transaction

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

    return render(request, 'login.html', {'MEDIA_URL':settings.MEDIA_URL})


def user_create(request):
    if request.method == 'POST':
        new_username = request.POST.get('new_username')
        new_password = request.POST.get('new_password')

        try:
            # 新しいユーザーオブジェクトを作成し、ユーザー名とハッシュ化されたパスワードを設定
            user = User.objects.create_user(username=new_username, password=new_password)
            # ログメッセージ作成
            logger.info('ユーザーの作成に成功しました。ユーザー名: %s', new_username)
            Tag.objects.create(user=user, tag_role='通路1')
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


from django.views.decorators.csrf import csrf_exempt


@csrf_exempt
def unity_login(request):
    if request.method == 'POST':
        try:
            data = json.loads(request.body)  # JSONデータをロード
            username = data.get('username')
            password = data.get('password')
            user = authenticate(request, username=username, password=password)
            if user is not None:
                login(request, user)
                return JsonResponse({"status": "ok"})
            else:
                return JsonResponse({"status": "fail"})
        except json.JSONDecodeError:
            return JsonResponse({"status": "error", "message": "Invalid JSON"})
    else:
        return JsonResponse({"status": "error", "message": "Only POST method is allowed"})


@login_required
def index(request):
    form = PhotoForm(user=request.user)
    tag_form = TagForm(user=request.user)

    tags = Tag.objects.filter(user=request.user)
    photos = Photo.objects.filter(user=request.user).order_by('photo_num')
    for tag in tags:
        print(f"Tag: {tag.tag_role}, Tag Name: {tag.name}")

    return render(request, 'index.html', {
        'form': form,
        'tag_form': tag_form,
        'tags': tags,
        'photos': photos,
        'MEDIA_URL': settings.MEDIA_URL
    })

class TagCreateView(generic.CreateView):
    model = Tag
    form_class = TagForm
    template_name = 'index.html'
    success_url = reverse_lazy('title')

    def form_valid(self, form):
        form.instance.user = self.request.user
        return super().form_valid(form)


class TagEditView(generic.UpdateView):
    model = Tag
    form_class = TagForm
    template_name = 'index.html'
    success_url = reverse_lazy('title')

class TagDeleteView(generic.DeleteView):
    def test_func(self):
        tag = get_object_or_404(Tag, pk=self.kwargs['pk'])
        return self.request.user.username == tag.user

    def post(self, request, *args, **kwargs):
        tag = get_object_or_404(Tag, pk=self.kwargs['pk'])
        tag.delete()
        return redirect('title')  # インデックスページにリダイレクト


class CreatePhotoView(generic.CreateView):
    model = Photo
    form_class = PhotoForm
    template_name = 'index.html'

    def get_form_kwargs(self):
        kwargs = super().get_form_kwargs()
        kwargs['user'] = self.request.user
        return kwargs

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context['tag_form'] = TagForm(user=self.request.user)
        context['obj'] = Photo.objects.filter(user=self.request.user).order_by('photo_num')
        context['MEDIA_URL'] = settings.MEDIA_URL
        return context

    def form_valid(self, form):
        with transaction.atomic():
            photo = form.save(commit=False)
            photo.user = self.request.user

            image_file = self.request.FILES['content']
            if isinstance(image_file, InMemoryUploadedFile):
                image = Image.open(io.BytesIO(image_file.read()))
                photo.width, photo.height = image.size

            photo.tag = self.request.POST.get('tag')
            new_photo_num = int(self.request.POST.get('photo_num'))
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


class DeletePhotoView(generic.DeleteView):
    def test_func(self):
        photo = get_object_or_404(Photo, pk=self.kwargs['pk'])
        return self.request.user.username == photo.user

    def post(self, request, *args, **kwargs):
        photo = get_object_or_404(Photo, pk=self.kwargs['pk'])
        photo.delete()
        return redirect('title')  # インデックスページにリダイレクト


class PhotoModelListView(APIView):
    serializer_class = PhotoSerializer
    permission_classes = [permissions.AllowAny]

    def get_queryset(self, username):
        User = get_user_model()
        user = get_object_or_404(User, username=username)
        return Photo.objects.filter(user=user).order_by('photo_num')

    def get(self, request, pk):
        queryset = self.get_queryset(pk)  # pkはURLから取得したユーザー名
        serializer = self.serializer_class(queryset, many=True)
        return Response(serializer.data)


class TagModelListView(APIView):
    permission_classes = [permissions.AllowAny]

    def get_queryset(self, username):
        User = get_user_model()
        user = get_object_or_404(User, username=username)
        return Tag.objects.filter(user=user).order_by('name')
    
    def get(self, request, pk):
        queryset = self.get_queryset(pk)
        serializer = TagSerializer(queryset, many=True)
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



