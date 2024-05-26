from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from django.contrib.auth import views as auth_views
from django.urls import include, path
from rest_framework.routers import DefaultRouter

from vr_museum_app.views import (PhotoModelListView, PhotoViewSet,
                                 UserModelListView, index, user_create,
                                 user_login)

router = DefaultRouter()
router.register(r'photos', PhotoViewSet)

urlpatterns = [
    path('', include(router.urls)),
    path('admin/', admin.site.urls),
    path('login/', user_login, name='login'),
    path('login_create/', user_create, name='login_create'),
    path('logout/', auth_views.LogoutView.as_view(), name='logout'),
    path('home/', index, name='title'),
    path('api/photo_model/', PhotoModelListView.as_view(), name='photo_model_list'),
    path('api/user_model/', UserModelListView.as_view(), name='user_model_list'),
] + static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)