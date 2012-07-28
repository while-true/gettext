GettextClass = function() {
    return this;
};

GettextClass.sep = "\004";

// singular and plural handler
GettextClass.prototype._ = function (msgid, msgidPlural, n) {
    if (msgidPlural != undefined && n != undefined) {
        var plural = n == 1 ? 0 : 1;

        if (__gettext_translations != undefined) {
            
            var tr = __gettext_translations.Translations;
            
            if (tr[msgid] != null && tr[msgid].P == msgidPlural) {
                if (__gettext_plural_func != undefined && __gettext_plural_func != null) {
                    plural = __gettext_plural_func(n);
                }
                
                return tr[msgid].T[plural];
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

String.prototype.formatWith = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
      ? args[number]
      : match
    ;
    });
};

String.prototype.formatWithNamed = function () {
    var args = arguments;
    var obj = args[0];
    return this.replace(/{(\w+)}/g, function (match, key) {
        return typeof obj[key] != 'undefined'
      ? obj[key]
      : match
    ;
    });
};

var __gettext_plural_func = function (n) { return n == 1 ? 0 : 1; };
var __gettext_translations = { "Translations": {}};
/* == TRANSLATION == */

var Gettext = new GettextClass();
