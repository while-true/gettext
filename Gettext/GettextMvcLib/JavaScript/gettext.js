GettextClass = function() {
    return this;
};

GettextClass.sep = "\004";

GettextClass.prototype._ = function (msgid) {
    if (__gettext_translations != undefined) {
        if (__gettext_translations.Translations[msgid] != null) {
            return __gettext_translations.Translations[msgid].T[0];
        }
    }

    return msgid;
};

var __gettext_translations = { "Translations": {}};
/* == TRANSLATION == */

var Gettext = new GettextClass();
