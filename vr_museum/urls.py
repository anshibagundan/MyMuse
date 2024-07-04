from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from django.contrib.auth import views as auth_views
from django.urls import include, path
from rest_framework.routers import DefaultRouter

from vr_museum_app.views import (PhotoModelListView, PhotoViewSet,
                                 UserModelListView, index, user_create, unity_login,
                                 user_login, TagCreateView, TagEditView, TagDeleteView, CreatePhotoView,
                                 DeletePhotoView)

router = DefaultRouter()
router.register(r'photos', PhotoViewSet)

urlpatterns = [
    path('', include(router.urls)),
    path('admin/', admin.site.urls),
    path('login/', user_login, name='login'),
    path('login_create/', user_create, name='login_create'),
    path('logout/', auth_views.LogoutView.as_view(), name='logout'),

    path('unity/login/', unity_login, name='unity_login'),

    path('home/', index, name='title'),

    path('tag/create/', TagCreateView.as_view(), name='tag_create'),
    path('tag/<int:pk>/edit/', TagEditView.as_view(), name='tag_edit'),
    path('tag/<int:pk>/delete/', TagDeleteView.as_view(), name='tag_delete'),

    path('photo/create/', CreatePhotoView.as_view(), name='photo_create'),
    path('photo/<int:pk>/delete/', DeletePhotoView.as_view(), name='photo_delete'),

    path('api/photo_model/<int:pk>/', PhotoModelListView.as_view(), name='photo_model_list'),
    path('api/user_model/', UserModelListView.as_view(), name='user_model_list'),
] + static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)