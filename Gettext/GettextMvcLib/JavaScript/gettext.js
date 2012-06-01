GettextClass = function() {
    return this;
};

GettextClass.sep = "\004";

GettextClass.prototype._ = function (msgid, msgidPlural, n) {
    if (msgidPlural != undefined && n != undefined) {
        var plural = n == 1 ? 0 : 1;

        if (__gettext_translations != undefined) {
            if (__gettext_plural_func != undefined && __gettext_plural_func != null) {
                plural = __gettext_plural_func(n);
            }
            
            if (__gettext_translations.Translations[msgid] != null) {
                return __gettext_translations.Translations[msgid].T[plural];
            }
        }

        var t = [msgid, msgidPlural];
        return t[plural];
    }


    if (__gettext_translations != undefined) {
        if (__gettext_translations.Translations[msgid] != null) {
            return __gettext_translations.Translations[msgid].T[0];
        }
    }

    return msgid;
};

var __gettext_plural_func = function (n) { return n == 1 ? 0 : 1; };
var __gettext_translations = { "Translations": {}};
/* == TRANSLATION == */

var Gettext = new GettextClass();
