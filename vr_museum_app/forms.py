from django import forms
from django.core.validators import MaxValueValidator, MinValueValidator

from .models import Photo, Tag


class PhotoForm(forms.ModelForm):
    tag = forms.ChoiceField(choices=[], required=True, label='タグ')
    photo_num = forms.ChoiceField(choices=[], required=True, label='写真番号')

    class Meta:
        model = Photo
        fields = ('title', 'detailed_title', 'time', 'content')
        widgets = {
            'title': forms.TextInput(attrs={'class': 'form-control'}),
            'detailed_title': forms.TextInput(attrs={'class': 'form-control'}),
            'time': forms.DateInput(attrs={'class': 'form-control', 'type': 'date'}),
            'content': forms.FileInput(attrs={'class': 'form-control'}),
        }

    def __init__(self, *args, **kwargs):
        user = kwargs.pop('user', None)  # Extract the user from kwargs
        super().__init__(*args, **kwargs)
        
        # Set choices for photo_num field
        photo_num_choices = [(i, str(i)) for i in range(1, Photo.objects.count() + 2)]
        self.fields['photo_num'].choices = photo_num_choices
        
        # Set choices for tag field
        if user:
            tag_choices = [(tag.tag, tag.tag) for tag in Tag.objects.filter(user=user)]
            self.fields['tag'].choices = tag_choices

class TagForm(forms.ModelForm):
    class Meta:
        model = Tag
        fields = ('tag',)
        widgets = {
            'tag': forms.TextInput(attrs={'class': 'form-control'}),
        }

    def __init__(self, *args, **kwargs):
        username = kwargs.pop('user', None)
        super().__init__(*args, **kwargs)

        # r タグの個数を取得
        r_count = Tag.objects.filter(tag__startswith='r', user=username).count() + 1
        # s タグの個数を取得
        s_count = Tag.objects.filter(tag__startswith='s', user=username).count() + 1

        # 選択肢を動的に設定する
        choices = [
            ('r{}'.format(r_count), 'r{}'.format(r_count)),
            ('s{}'.format(s_count), 's{}'.format(s_count)),
        ]
        # tag フィールドの選択肢を更新する
        self.fields['tag'].widget = forms.Select(
            choices=[(option, option) for option in [choice[1] for choice in choices]],
            attrs={'class': 'form-control'}
        )

class TagForm_delete(forms.ModelForm):
    tag = forms.ModelChoiceField(queryset=Tag.objects.none(), required=True, label='Tag', widget=forms.Select(attrs={'class': 'form-control'}))
    class Meta:
        model = Tag
        fields = ('tag',)
        widgets = {
            'tag': forms.CheckboxSelectMultiple(),  # チェックボックスを使用する
        }
    def __init__(self, *args, **kwargs):
        username = kwargs.pop('user', None)
        super().__init__(*args, **kwargs)
        # タグの選択肢を設定する
        self.fields['tag'].queryset = Tag.objects.filter(user=username)