from django import template

register = template.Library()

@register.filter
def first_match_tag(photos, tag_role):
    for photo in photos:
        if photo.tag == tag_role:
            return True
    return False