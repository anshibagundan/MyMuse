from django import forms
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
            tag_choices = [(tag.tag_role, tag.tag_role) for tag in Tag.objects.filter(user=user)]
            self.fields['tag'].choices = tag_choices


class TagForm(forms.ModelForm):

    ROOM_CHOICES = [
        ('normal', 'ノーマル'),
        ('spring', '春'),
        ('summer', '夏'),
        ('autumn', '秋'),
        ('winter', '冬'),
    ]

    name = forms.CharField(max_length=20, label='Tag Name')
    room_kinds = forms.ChoiceField(choices=ROOM_CHOICES, label='Room Kind')

    class Meta:
        model = Tag
        fields = ('tag_role','name','room_kinds')
        widgets = {
            'tag_role': forms.TextInput(attrs={'class': 'form-control'}),
            'name': forms.TextInput(attrs={'class': 'form-control'}),
            'room_kinds': forms.Select(attrs={'class': 'form-control'}),
        }

    def __init__(self, *args, **kwargs):
        username = kwargs.pop('user', None)
        super().__init__(*args, **kwargs)

        # r タグの個数を取得
        r_count = Tag.objects.filter(tag_role__startswith='部屋', user=username).count() + 1
        # s タグの個数を取得
        s_count = Tag.objects.filter(tag_role__startswith='通路', user=username).count() + 1

        # 選択肢を動的に設定する
        choices = [
            ('部屋{}'.format(r_count), '部屋{}'.format(r_count)),
            ('通路{}'.format(s_count), '通路{}'.format(s_count)),
        ]

        # tag_role フィールドの選択肢を更新し、onClick イベントを追加
        self.fields['tag_role'].widget = forms.Select(
            choices=[(option, option) for option in [choice[1] for choice in choices]],
            attrs={
                'class': 'form-control',
                'onChange': "handleTagRoleClick()"
            }
        )


class TagFormDelete(forms.ModelForm):
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