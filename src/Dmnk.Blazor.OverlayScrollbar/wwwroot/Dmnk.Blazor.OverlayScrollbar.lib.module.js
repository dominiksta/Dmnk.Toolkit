import {
    OverlayScrollbars
} from '/_content/Dmnk.Blazor.OverlayScrollbar/overlayscrollbars.esm.min.js';

/**
 * For enhanced navigation page loads, (or for that matter any other sort of
 * dynamically loaded and unloaded content) we need to destroy and re-apply the
 * scrollbar script after every load.
 * The most straightforward way to do this seems to be defining a web component.
 */
export class OverlayScrollbarComponent extends HTMLElement {
    #osInstance = undefined;
    
    connectedCallback() {
        const _ = this.connect();
    }

    /**
     * This definitely looks a little awkward, but it seems to be necessary:
     * When this web component is loaded through blazor wasm, the child
     * components seem to not be there most of the time when connectedCallback
     * is executing. We therefore have to wait for the child components to
     * actually be there.
     */
    async connect() {
        const maxRetries = 20;
        let retries = 0;
        while (!this.firstElementChild) {
            if (retries >= maxRetries) {
                console.error('scrollbar could not connect - first child missing');
                return;
            }
            retries++;
            await new Promise(resolve => setTimeout(resolve, 10));
        }
        // console.debug(
        //     'scrollbar first child', 
        //     { retries, child: this.firstElementChild }
        // );
        this.#osInstance = OverlayScrollbars(this, {});
    }

    disconnectedCallback() {
        // console.log('disconnected');
        if (this.#osInstance !== undefined) this.#osInstance.destroy();
    }
}

customElements.define('r-overlay-scrollbar', OverlayScrollbarComponent);