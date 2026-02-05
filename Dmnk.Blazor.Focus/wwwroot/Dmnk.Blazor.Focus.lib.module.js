window.__Dmnk_Blazor_Focus = class {
    static #saveFocusMap = new Map();

    /**
     * @param {string} id
     * @return void
     */
    static SaveFocus(id) {
        this.#saveFocusMap.set(id, document.activeElement);
    }

    /**
     * @param {string} id
     * @return void
     */
    static RestoreFocus(id) {
        const last = this.#saveFocusMap.get(id);
        if (last) last.focus();
        else console.warn('could not get last focused for id ', id);
    }

    /**
     * @param {HTMLElement} el
     * @param {number} maxRetries
     * @return {Promise<void>}
     */
    static async ForceFocus(el, maxRetries) {
        const initialTimeout = 100, timeout = 10;
        el?.focus();
        await new Promise(resolve => setTimeout(resolve, initialTimeout));
        let retries = 0;
        while(retries < maxRetries && document.activeElement !== el) {{
            retries++;
            el?.focus();
            await new Promise(resolve => setTimeout(resolve, timeout));
        }}
        if (document.activeElement !== el) {{
            console.warn(`Could not focus element after ${{retries}} attempts`, el);
        }}
    }

    /**
     * @param {HTMLElement} el
     * @return {void}
     */
    static FocusFirst(el) {
        const found = this.#findFocusable(el);
        // console.debug(found);
        if (found.length === 0) return;
        found[0].focus();
    }

    /**
     * @param {HTMLElement} el
     * @return {void}
     */
    static FocusLast(el) {
        const found = this.#findFocusable(el);
        // console.debug(found);
        if (found.length === 0) return;
        found[found.length - 1].focus();
    }

    /**
     * Kind of silly, but in order to find anything focusable in web
     * components (like those of fluent-ui), we have to look into every
     * shadow root recursively. There is sadly no way to just `querySelector`
     * through shadow roots.
     * 
     * @param {ShadowRoot|Element} el
     * @param {number} stackSize
     * @return {ShadowRoot[]}
     */
    static #collectShadowRoots(el, stackSize = 0) {
        let ret = [];
        // limit recursion depth so we don't accidentally get a stack overflow
        // - a wrong focus is better than a hard error
        if (stackSize > 20) return ret;
        for (const child of el.children) {{
            if (child.shadowRoot) {{
                ret.push(child.shadowRoot);
                ret = ret.concat(...this.#collectShadowRoots(child.shadowRoot, stackSize + 1));
            }}
            ret = ret.concat(...this.#collectShadowRoots(child, stackSize + 1));
        }}
        return ret;
    }

    /**
     * This should capture most all elements that can be focused - not
     * including elements in shadow roots. The output is ordered by tabindex.
     * 
     * @param {HTMLElement} el
     * @return {HTMLElement[]}
     */
    static #findFocusableSingle(el) {
        /** @type {HTMLElement[]} */
        let found = Array.from(el.querySelectorAll(
            '[tabindex], input:not([type="hidden"]), ' +
            'select, textarea, button, object, a'
        ));
        found = found
            .filter(el => el.tabIndex !== -1)
            .sort((a, b) => (a.tabIndex ?? 0) - (b.tabIndex ?? 0));
        return found;
    }
    
    /**
     * Same as {{@link this.#findFocusableSingle}}, but includes elements in
     * shadow roots using {{@link this.#collectShadowRoots}}.
     * 
     * @param {HTMLElement} el
     * @return {HTMLElement[]}
     */
    static #findFocusable(el) {
        let ret = this.#findFocusableSingle(el);
        const shadowRoots = this.#collectShadowRoots(el);
        for (const shadowRoot of shadowRoots) {
            ret = ret.concat(...this.#findFocusableSingle(shadowRoot));
        }
        return ret;
    }
}
