(()=>{"use strict";const t=Symbol("defaultState"),e=Symbol("delegatesFocus"),s=Symbol("firstRender"),r=Symbol("focusTarget"),n=Symbol("hasDynamicTemplate"),a=Symbol("ids"),o=Symbol("nativeInternals"),i=Symbol("raiseChangeEvents"),l=Symbol("render"),d=Symbol("renderChanges"),c=Symbol("rendered"),u=Symbol("rendering"),h=Symbol("setState"),p=Symbol("shadowRoot"),y=Symbol("shadowRootMode"),m=Symbol("state"),g=Symbol("stateEffects"),b=Symbol("template"),f=Symbol("mousedownListener");function w(t,e){return"boolean"==typeof e?e:"string"==typeof e&&(""===e||t.toLowerCase()===e.toLowerCase())}function v(t){for(const e of function*(t){t&&(yield t,yield*function*(t){let e=t;for(;e=e instanceof HTMLElement&&e.assignedSlot?e.assignedSlot:e instanceof ShadowRoot?e.host:e.parentNode,e;)yield e}(t))}(t)){const t=e[r]||e,s=t;if(t instanceof HTMLElement&&t.tabIndex>=0&&!s.disabled&&!(t instanceof HTMLSlotElement))return t}return null}function S(t,e){let s=e;for(;s;){const e=s.assignedSlot||s.parentNode||s.host;if(e===t)return!0;s=e}return!1}function T(t){const e=P(t,(t=>t instanceof HTMLElement&&t.matches('a[href],area[href],button:not([disabled]),details,iframe,input:not([disabled]),select:not([disabled]),textarea:not([disabled]),[contentEditable="true"],[tabindex]')&&t.tabIndex>=0)),{value:s}=e.next();return s instanceof HTMLElement?s:null}function x(t,e){t[f]&&t.removeEventListener("mousedown",t[f]),e&&(t[f]=t=>{if(0!==t.button)return;const s=v(e[r]||e);s&&(s.focus(),t.preventDefault())},t.addEventListener("mousedown",t[f]))}function k(t,e,s){t.toggleAttribute(e,s),t[o]&&t[o].states&&t[o].states.toggle(e,s)}const E={checked:!0,defer:!0,disabled:!0,hidden:!0,ismap:!0,multiple:!0,noresize:!0,readonly:!0,selected:!0};function*P(t,e){let s;if(e(t)&&(yield t),t instanceof HTMLElement&&t.shadowRoot)s=t.shadowRoot.children;else{const e=t instanceof HTMLSlotElement?t.assignedNodes({flatten:!0}):[];s=e.length>0?e:t.childNodes}if(s)for(let t=0;t<s.length;t++)yield*P(s[t],e)}const D=(t,...e)=>C.html(t,...e).content,C={html(t,...e){const s=document.createElement("template");return s.innerHTML=String.raw(t,...e),s}},A={tabindex:"tabIndex"},L={tabIndex:"tabindex"};function M(t){if(t===HTMLElement)return[];const e=Object.getPrototypeOf(t.prototype).constructor;let s=e.observedAttributes;s||(s=M(e));const r=Object.getOwnPropertyNames(t.prototype).filter((e=>{const s=Object.getOwnPropertyDescriptor(t.prototype,e);return s&&"function"==typeof s.set})).map((t=>function(t){let e=L[t];if(!e){const s=/([A-Z])/g;e=t.replace(s,"-$1").toLowerCase(),L[t]=e}return e}(t))).filter((t=>s.indexOf(t)<0));return s.concat(r)}const O=Symbol("state"),B=Symbol("raiseChangeEventsInNextRender"),I=Symbol("changedSinceLastRender");function H(t,e){const s={};for(const a in e)r=e[a],n=t[a],(r instanceof Date&&n instanceof Date?r.getTime()===n.getTime():r===n)||(s[a]=!0);var r,n;return s}const N=new Map,F=Symbol("shadowIdProxy"),R=Symbol("proxyElement"),j={get(t,e){const s=t[R][p];return s&&"string"==typeof e?s.getElementById(e):null}};function Y(t){let e=t[n]?void 0:N.get(t.constructor);if(void 0===e&&(e=t[b],e)){if(!(e instanceof HTMLTemplateElement))throw`Warning: the [template] property for ${t.constructor.name} must return an HTMLTemplateElement.`;t[n]||N.set(t.constructor,e)}return e}const W=function(t){return class extends t{attributeChangedCallback(t,e,s){if(super.attributeChangedCallback&&super.attributeChangedCallback(t,e,s),s!==e&&!this[u]){const e=function(t){let e=A[t];if(!e){const s=/-([a-z])/g;e=t.replace(s,(t=>t[1].toUpperCase())),A[t]=e}return e}(t);if(e in this){const r=E[t]?w(t,s):s;this[e]=r}}}static get observedAttributes(){return M(this)}}}(function(e){class r extends e{constructor(){super(),this[s]=void 0,this[i]=!1,this[I]=null,this[h](this[t])}connectedCallback(){super.connectedCallback&&super.connectedCallback(),this[d]()}get[t](){return super[t]||{}}[l](t){super[l]&&super[l](t)}[d](){void 0===this[s]&&(this[s]=!0);const t=this[I];if(this[s]||t){const e=this[i];this[i]=this[B],this[u]=!0,this[l](t),this[u]=!1,this[I]=null,this[c](t),this[s]=!1,this[i]=e,this[B]=e}}[c](t){super[c]&&super[c](t)}async[h](t){this[u]&&console.warn(this.constructor.name+" called [setState] during rendering, which you should avoid.\nSee https://elix.org/documentation/ReactiveMixin.");const{state:e,changed:r}=function(t,e){const s=Object.assign({},t[O]),r={};let n=e;for(;;){const e=H(s,n);if(0===Object.keys(e).length)break;Object.assign(s,n),Object.assign(r,e),n=t[g](s,e)}return{state:s,changed:r}}(this,t);if(this[O]&&0===Object.keys(r).length)return;Object.freeze(e),this[O]=e,this[i]&&(this[B]=!0);const n=void 0===this[s]||null!==this[I];this[I]=Object.assign(this[I]||{},r),this.isConnected&&!n&&(await Promise.resolve(),this[d]())}get[m](){return this[O]}[g](t,e){return super[g]?super[g](t,e):{}}}return"true"===new URLSearchParams(location.search).get("elixdebug")&&Object.defineProperty(r.prototype,"state",{get(){return this[m]}}),r}(function(t){return class extends t{get[a](){if(!this[F]){const t={[R]:this};this[F]=new Proxy(t,j)}return this[F]}[l](t){if(super[l]&&super[l](t),void 0===this[s]||this[s]){const t=Y(this);if(t){const s=this.attachShadow({delegatesFocus:this[e],mode:this[y]}),r=document.importNode(t.content,!0);s.append(r),this[p]=s}}}get[y](){return"open"}}}(HTMLElement))),V=new Map;function G(t){if("function"==typeof t){let e;try{e=new t}catch(s){if("TypeError"!==s.name)throw s;!function(t){let e;const s=t.name&&t.name.match(/^[A-Za-z][A-Za-z0-9_$]*$/);if(s){const t=/([A-Z])/g;e=s[0].replace(t,((t,e,s)=>s>0?"-"+e:e)).toLowerCase()}else e="custom-element";let r,n=V.get(e)||0;for(;r=`${e}-${n}`,customElements.get(r);n++);customElements.define(r,t),V.set(e,n+1)}(t),e=new t}return e}return document.createElement(t)}function U(t,e){if("function"==typeof e&&t.constructor===e||"string"==typeof e&&t instanceof Element&&t.localName===e)return t;{const s=G(e);return function(t,e){const s=t.parentNode;if(!s)throw"An element must have a parent before it can be substituted.";(t instanceof HTMLElement||t instanceof SVGElement)&&(e instanceof HTMLElement||e instanceof SVGElement)&&(Array.prototype.forEach.call(t.attributes,(t=>{e.getAttribute(t.name)||"class"===t.name||"style"===t.name||e.setAttribute(t.name,t.value)})),Array.prototype.forEach.call(t.classList,(t=>{e.classList.add(t)})),Array.prototype.forEach.call(t.style,(s=>{e.style[s]||(e.style[s]=t.style[s])}))),e.append(...t.childNodes),s.replaceChild(e,t)}(t,s),s}}Symbol("applyElementData");const K=Symbol("checkSize"),z=Symbol("closestAvailableItemIndex"),Z=Symbol("contentSlot"),$=t,q=Symbol("defaultTabIndex"),J=e,Q=Symbol("effectEndTarget"),X=s,_=r,tt=Symbol("getItemText"),et=Symbol("goDown"),st=Symbol("goEnd"),rt=Symbol("goFirst"),nt=Symbol("goLast"),at=Symbol("goLeft"),ot=Symbol("goNext"),it=Symbol("goPrevious"),lt=Symbol("goRight"),dt=Symbol("goStart"),ct=Symbol("goToItemWithPrefix"),ut=Symbol("goUp"),ht=n,pt=a,yt=Symbol("inputDelegate"),mt=Symbol("itemsDelegate"),gt=Symbol("keydown"),bt=(Symbol("matchText"),Symbol("mouseenter")),ft=Symbol("mouseleave"),wt=o,vt=i,St=l,Tt=d,xt=Symbol("renderDataToElement"),kt=c,Et=u,Pt=Symbol("scrollTarget"),Dt=h,Ct=p,At=y,Lt=Symbol("startEffect"),Mt=m,Ot=g,Bt=Symbol("swipeDown"),It=Symbol("swipeDownComplete"),Ht=Symbol("swipeLeft"),Nt=Symbol("swipeLeftTransitionEnd"),Ft=Symbol("swipeRight"),Rt=Symbol("swipeRightTransitionEnd"),jt=Symbol("swipeUp"),Yt=Symbol("swipeUpComplete"),Wt=Symbol("swipeStart"),Vt=Symbol("swipeTarget"),Gt=Symbol("tap"),Ut=b,Kt=Symbol("toggleSelectedFlag");"true"===new URLSearchParams(location.search).get("elixdebug")&&(window.elix={internal:{checkSize:K,closestAvailableItemIndex:z,contentSlot:Z,defaultState:$,defaultTabIndex:q,delegatesFocus:J,effectEndTarget:Q,firstRender:X,focusTarget:_,getItemText:tt,goDown:et,goEnd:st,goFirst:rt,goLast:nt,goLeft:at,goNext:ot,goPrevious:it,goRight:lt,goStart:dt,goToItemWithPrefix:ct,goUp:ut,hasDynamicTemplate:ht,ids:pt,inputDelegate:yt,itemsDelegate:mt,keydown:gt,mouseenter:bt,mouseleave:ft,nativeInternals:wt,event,raiseChangeEvents:vt,render:St,renderChanges:Tt,renderDataToElement:xt,rendered:kt,rendering:Et,scrollTarget:Pt,setState:Dt,shadowRoot:Ct,shadowRootMode:At,startEffect:Lt,state:Mt,stateEffects:Ot,swipeDown:Bt,swipeDownComplete:It,swipeLeft:Ht,swipeLeftTransitionEnd:Nt,swipeRight:Ft,swipeRightTransitionEnd:Rt,swipeUp:jt,swipeUpComplete:Yt,swipeStart:Wt,swipeTarget:Vt,tap:Gt,template:Ut,toggleSelectedFlag:Kt}});const zt=document.createElement("div");zt.attachShadow({mode:"open",delegatesFocus:!0});const Zt=zt.shadowRoot.delegatesFocus;function $t(t){if("selectedText"in t)return t.selectedText;if("value"in t&&"options"in t){const e=t.value,s=t.options.find((t=>t.value===e));return s?s.innerText:""}return"value"in t?t.value:t.innerText}function qt(t,e){const{ariaLabel:s,ariaLabelledby:r}=e,n=t.isConnected?t.getRootNode():null;let a=null;if(r&&n)a=r.split(" ").map((s=>{const r=n.getElementById(s);return r?r===t&&null!==e.value?e.selectedText:$t(r):""})).join(" ");else if(s)a=s;else if(n){const e=t.id;if(e){const t=n.querySelector(`[for="${e}"]`);t instanceof HTMLElement&&(a=$t(t))}if(null===a){const e=t.closest("label");e&&(a=$t(e))}}return a&&(a=a.trim()),a}let Jt=!1;const Qt=Symbol("focusVisibleChangedListener");function Xt(t){return class extends t{constructor(){super(),this.addEventListener("focusout",(t=>{Promise.resolve().then((()=>{const e=t.relatedTarget||document.activeElement,s=this===e,r=S(this,e);!s&&!r&&(this[Dt]({focusVisible:!1}),document.removeEventListener("focusvisiblechange",this[Qt]),this[Qt]=null)}))})),this.addEventListener("focusin",(()=>{Promise.resolve().then((()=>{this[Mt].focusVisible!==Jt&&this[Dt]({focusVisible:Jt}),this[Qt]||(this[Qt]=()=>{this[Dt]({focusVisible:Jt})},document.addEventListener("focusvisiblechange",this[Qt]))}))}))}get[$](){return Object.assign(super[$]||{},{focusVisible:!1})}[St](t){if(super[St]&&super[St](t),t.focusVisible){const{focusVisible:t}=this[Mt];this.toggleAttribute("focus-visible",t)}}get[Ut](){const t=super[Ut]||C.html``;return t.content.append(D`
        <style>
          :host {
            outline: none;
          }

          :host([focus-visible]:focus-within) {
            outline-color: Highlight; /* Firefox */
            outline-color: -webkit-focus-ring-color; /* All other browsers */
            outline-style: auto;
          }
        </style>
      `),t}}}function _t(t){if(Jt!==t){Jt=t;const e=new CustomEvent("focus-visible-changed",{detail:{focusVisible:Jt}});document.dispatchEvent(e);const s=new CustomEvent("focusvisiblechange",{detail:{focusVisible:Jt}});document.dispatchEvent(s)}}window.addEventListener("keydown",(()=>{_t(!0)}),{capture:!0}),window.addEventListener("mousedown",(()=>{_t(!1)}),{capture:!0});const te=Symbol("extends"),ee=Symbol("delegatedPropertySetters"),se={a:!0,area:!0,button:!0,details:!0,iframe:!0,input:!0,select:!0,textarea:!0},re={address:["scroll"],blockquote:["scroll"],caption:["scroll"],center:["scroll"],dd:["scroll"],dir:["scroll"],div:["scroll"],dl:["scroll"],dt:["scroll"],fieldset:["scroll"],form:["reset","scroll"],frame:["load"],h1:["scroll"],h2:["scroll"],h3:["scroll"],h4:["scroll"],h5:["scroll"],h6:["scroll"],iframe:["load"],img:["abort","error","load"],input:["abort","change","error","select","load"],li:["scroll"],link:["load"],menu:["scroll"],object:["error","scroll"],ol:["scroll"],p:["scroll"],script:["error","load"],select:["change","scroll"],tbody:["scroll"],tfoot:["scroll"],thead:["scroll"],textarea:["change","select","scroll"]},ne=["click","dblclick","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup","wheel"],ae={abort:!0,change:!0,reset:!0},oe=["address","article","aside","blockquote","canvas","dd","div","dl","fieldset","figcaption","figure","footer","form","h1","h2","h3","h4","h5","h6","header","hgroup","hr","li","main","nav","noscript","ol","output","p","pre","section","table","tfoot","ul","video"],ie=["accept-charset","autoplay","buffered","challenge","codebase","colspan","contenteditable","controls","crossorigin","datetime","dirname","for","formaction","http-equiv","icon","ismap","itemprop","keytype","language","loop","manifest","maxlength","minlength","muted","novalidate","preload","radiogroup","readonly","referrerpolicy","rowspan","scoped","usemap"],le=function(t){return class extends t{get[J](){return!0}focus(t){const e=this[_];e&&e.focus(t)}get[_](){return T(this[Ct])}}}(W);class de extends le{constructor(){super();!this[wt]&&this.attachInternals&&(this[wt]=this.attachInternals())}attributeChangedCallback(t,e,s){if(ie.indexOf(t)>=0){const e=Object.assign({},this[Mt].innerAttributes,{[t]:s});this[Dt]({innerAttributes:e})}else super.attributeChangedCallback(t,e,s)}blur(){this.inner.blur()}get[$](){return Object.assign(super[$],{innerAttributes:{}})}get[q](){return se[this.extends]?0:-1}get extends(){return this.constructor[te]}get inner(){const t=this[pt]&&this[pt].inner;return t||console.warn("Attempted to get an inner standard element before it was instantiated."),t}getInnerProperty(t){return this[Mt][t]||this[Ct]&&this.inner[t]}static get observedAttributes(){return[...super.observedAttributes,...ie]}[St](t){super[St](t);const e=this.inner;if(this[X]&&((re[this.extends]||[]).forEach((t=>{e.addEventListener(t,(()=>{const e=new Event(t,{bubbles:ae[t]||!1});this.dispatchEvent(e)}))})),"disabled"in e&&ne.forEach((t=>{this.addEventListener(t,(t=>{e.disabled&&t.stopImmediatePropagation()}))}))),t.tabIndex&&(e.tabIndex=this[Mt].tabIndex),t.innerAttributes){const{innerAttributes:t}=this[Mt];for(const s in t)ce(e,s,t[s])}this.constructor[ee].forEach((s=>{if(t[s]){const t=this[Mt][s];("selectionEnd"===s||"selectionStart"===s)&&null===t||(e[s]=t)}}))}[kt](t){if(super[kt](t),t.disabled){const{disabled:t}=this[Mt];void 0!==t&&k(this,"disabled",t)}}setInnerProperty(t,e){this[Mt][t]!==e&&this[Dt]({[t]:e})}get[Ut](){const t=oe.includes(this.extends)?"block":"inline-block";return C.html`
      <style>
        :host {
          display: ${t}
        }
        
        [part~="inner"] {
          box-sizing: border-box;
          height: 100%;
          width: 100%;
        }
      </style>
      <${this.extends} id="inner" part="inner">
        <slot></slot>
      </${this.extends}>
    `}static wrap(t){class e extends de{}e[te]=t;const s=document.createElement(t);return function(t,e){const s=Object.getOwnPropertyNames(e);t[ee]=[],s.forEach((s=>{const r=Object.getOwnPropertyDescriptor(e,s);if(!r)return;const n=function(t,e){if("function"==typeof e.value){if("constructor"!==t)return function(t,e){return{configurable:e.configurable,enumerable:e.enumerable,value:function(...e){this.inner[t](...e)},writable:e.writable}}(t,e)}else if("function"==typeof e.get||"function"==typeof e.set)return function(t,e){const s={configurable:e.configurable,enumerable:e.enumerable};return e.get&&(s.get=function(){return this.getInnerProperty(t)}),e.set&&(s.set=function(e){this.setInnerProperty(t,e)}),e.writable&&(s.writable=e.writable),s}(t,e);return null}(s,r);n&&(Object.defineProperty(t.prototype,s,n),n.set&&t[ee].push(s))}))}(e,Object.getPrototypeOf(s)),e}}function ce(t,e,s){E[e]?"string"==typeof s?t.setAttribute(e,""):null===s&&t.removeAttribute(e):null!=s?t.setAttribute(e,s.toString()):t.removeAttribute(e)}const ue=function(t){return class extends t{get[$](){return Object.assign(super[$]||{},{composeFocus:!Zt})}[St](t){super[St]&&super[St](t),this[X]&&this.addEventListener("mousedown",(t=>{if(this[Mt].composeFocus&&0===t.button&&t.target instanceof Element){const e=v(t.target);e&&(e.focus(),t.preventDefault())}}))}}}(function(t){return class extends t{get ariaLabel(){return this[Mt].ariaLabel}set ariaLabel(t){this[Mt].removingAriaAttribute||this[Dt]({ariaLabel:t})}get ariaLabelledby(){return this[Mt].ariaLabelledby}set ariaLabelledby(t){this[Mt].removingAriaAttribute||this[Dt]({ariaLabelledby:t})}get[$](){return Object.assign(super[$]||{},{ariaLabel:null,ariaLabelledby:null,inputLabel:null,removingAriaAttribute:!1})}[St](t){if(super[St]&&super[St](t),this[X]&&this.addEventListener("focus",(()=>{this[vt]=!0;const t=qt(this,this[Mt]);this[Dt]({inputLabel:t}),this[vt]=!1})),t.inputLabel){const{inputLabel:t}=this[Mt];t?this[yt].setAttribute("aria-label",t):this[yt].removeAttribute("aria-label")}}[kt](t){super[kt]&&super[kt](t),this[X]&&(window.requestIdleCallback||setTimeout)((()=>{const t=qt(this,this[Mt]);this[Dt]({inputLabel:t})}));const{ariaLabel:e,ariaLabelledby:s}=this[Mt];t.ariaLabel&&!this[Mt].removingAriaAttribute&&this.getAttribute("aria-label")&&(this.setAttribute("delegated-label",e),this[Dt]({removingAriaAttribute:!0}),this.removeAttribute("aria-label")),t.ariaLabelledby&&!this[Mt].removingAriaAttribute&&this.getAttribute("aria-labelledby")&&(this.setAttribute("delegated-labelledby",s),this[Dt]({removingAriaAttribute:!0}),this.removeAttribute("aria-labelledby")),t.removingAriaAttribute&&this[Mt].removingAriaAttribute&&this[Dt]({removingAriaAttribute:!1})}[Ot](t,e){const s=super[Ot]?super[Ot](t,e):{};if(e.ariaLabel&&t.ariaLabel||e.selectedText&&t.ariaLabelledby&&this.matches(":focus-within")){const e=qt(this,t);Object.assign(s,{inputLabel:e})}return s}}}(Xt(de.wrap("button")))),he=class extends ue{get[$](){return Object.assign(super[$],{role:"button"})}get[yt](){return this[pt].inner}[Gt](){const t=new MouseEvent("click",{bubbles:!0,cancelable:!0});this.dispatchEvent(t)}get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            display: inline-flex;
            outline: none;
            -webkit-tap-highlight-color: transparent;
            touch-action: manipulation;
          }

          [part~="inner"] {
            align-items: center;
            background: none;
            border: none;
            color: inherit;
            flex: 1;
            font: inherit;
            outline: none;
            padding: 0;
          }
        </style>
      `),t}},pe=Symbol("wrap");function ye(t){return class extends t{get arrowButtonOverlap(){return this[Mt].arrowButtonOverlap}set arrowButtonOverlap(t){this[Dt]({arrowButtonOverlap:t})}get arrowButtonPartType(){return this[Mt].arrowButtonPartType}set arrowButtonPartType(t){this[Dt]({arrowButtonPartType:t})}arrowButtonPrevious(){return super.arrowButtonPrevious?super.arrowButtonPrevious():this[it]()}arrowButtonNext(){return super.arrowButtonNext?super.arrowButtonNext():this[ot]()}attributeChangedCallback(t,e,s){"arrow-button-overlap"===t?this.arrowButtonOverlap="true"===String(s):"show-arrow-buttons"===t?this.showArrowButtons="true"===String(s):super.attributeChangedCallback(t,e,s)}get[$](){return Object.assign(super[$]||{},{arrowButtonOverlap:!0,arrowButtonPartType:he,orientation:"horizontal",showArrowButtons:!0})}[St](t){if(t.arrowButtonPartType){const t=this[pt].arrowButtonPrevious;t instanceof HTMLElement&&x(t,null);const e=this[pt].arrowButtonNext;e instanceof HTMLElement&&x(e,null)}if(super[St]&&super[St](t),ge(this[Ct],this[Mt],t),t.arrowButtonPartType){const t=this,e=this[pt].arrowButtonPrevious;e instanceof HTMLElement&&x(e,t);const s=me(this,(()=>this.arrowButtonPrevious()));e.addEventListener("mousedown",s);const r=this[pt].arrowButtonNext;r instanceof HTMLElement&&x(r,t);const n=me(this,(()=>this.arrowButtonNext()));r.addEventListener("mousedown",n)}const{arrowButtonOverlap:e,canGoNext:s,canGoPrevious:r,orientation:n,rightToLeft:a}=this[Mt],o="vertical"===n,i=this[pt].arrowButtonPrevious,l=this[pt].arrowButtonNext;if(t.arrowButtonOverlap||t.orientation||t.rightToLeft){this[pt].arrowDirection.style.flexDirection=o?"column":"row";const t={bottom:null,left:null,right:null,top:null};let s,r;e?Object.assign(t,{position:"absolute","z-index":1}):Object.assign(t,{position:null,"z-index":null}),e&&(o?(Object.assign(t,{left:0,right:0}),s={top:0},r={bottom:0}):(Object.assign(t,{bottom:0,top:0}),a?(s={right:0},r={left:0}):(s={left:0},r={right:0}))),Object.assign(i.style,t,s),Object.assign(l.style,t,r)}if(t.canGoNext&&null!==s&&(l.disabled=!s),t.canGoPrevious&&null!==r&&(i.disabled=!r),t.showArrowButtons){const t=this[Mt].showArrowButtons?null:"none";i.style.display=t,l.style.display=t}}get showArrowButtons(){return this[Mt].showArrowButtons}set showArrowButtons(t){this[Dt]({showArrowButtons:t})}[pe](t){const e=D`
        <div
          id="arrowDirection"
          role="none"
          style="display: flex; flex: 1; overflow: hidden; position: relative;"
        >
          <div
            id="arrowButtonPrevious"
            part="arrow-button arrow-button-previous"
            exportparts="inner:arrow-button-inner"
            class="arrowButton"
            aria-hidden="true"
            tabindex="-1"
          >
            <slot name="arrowButtonPrevious"></slot>
          </div>
          <div
            id="arrowDirectionContainer"
            role="none"
            style="flex: 1; overflow: hidden; position: relative;"
          ></div>
          <div
            id="arrowButtonNext"
            part="arrow-button arrow-button-next"
            exportparts="inner:arrow-button-inner"
            class="arrowButton"
            aria-hidden="true"
            tabindex="-1"
          >
            <slot name="arrowButtonNext"></slot>
          </div>
        </div>
      `;ge(e,this[Mt]);const s=e.getElementById("arrowDirectionContainer");s&&(t.replaceWith(e),s.append(t))}}}function me(t,e){return async function(s){0===s.button&&(t[vt]=!0,e()&&s.stopPropagation(),await Promise.resolve(),t[vt]=!1)}}function ge(t,e,s){if(!s||s.arrowButtonPartType){const{arrowButtonPartType:s}=e,r=t.getElementById("arrowButtonPrevious");r&&U(r,s);const n=t.getElementById("arrowButtonNext");n&&U(n,s)}}ye.wrap=pe;const be=ye,fe={firstDay:{"001":1,AD:1,AE:6,AF:6,AG:0,AI:1,AL:1,AM:1,AN:1,AR:1,AS:0,AT:1,AU:0,AX:1,AZ:1,BA:1,BD:0,BE:1,BG:1,BH:6,BM:1,BN:1,BR:0,BS:0,BT:0,BW:0,BY:1,BZ:0,CA:0,CH:1,CL:1,CM:1,CN:0,CO:0,CR:1,CY:1,CZ:1,DE:1,DJ:6,DK:1,DM:0,DO:0,DZ:6,EC:1,EE:1,EG:6,ES:1,ET:0,FI:1,FJ:1,FO:1,FR:1,GB:1,"GB-alt-variant":0,GE:1,GF:1,GP:1,GR:1,GT:0,GU:0,HK:0,HN:0,HR:1,HU:1,ID:0,IE:1,IL:0,IN:0,IQ:6,IR:6,IS:1,IT:1,JM:0,JO:6,JP:0,KE:0,KG:1,KH:0,KR:0,KW:6,KZ:1,LA:0,LB:1,LI:1,LK:1,LT:1,LU:1,LV:1,LY:6,MC:1,MD:1,ME:1,MH:0,MK:1,MM:0,MN:1,MO:0,MQ:1,MT:0,MV:5,MX:0,MY:1,MZ:0,NI:0,NL:1,NO:1,NP:0,NZ:1,OM:6,PA:0,PE:0,PH:0,PK:0,PL:1,PR:0,PT:0,PY:0,QA:6,RE:1,RO:1,RS:1,RU:1,SA:0,SD:6,SE:1,SG:0,SI:1,SK:1,SM:1,SV:0,SY:6,TH:0,TJ:1,TM:1,TR:1,TT:0,TW:0,UA:1,UM:0,US:0,UY:1,UZ:1,VA:1,VE:0,VI:0,VN:1,WS:0,XK:1,YE:0,ZA:0,ZW:0},weekendEnd:{"001":0,AE:6,AF:5,BH:6,DZ:6,EG:6,IL:6,IQ:6,IR:5,JO:6,KW:6,LY:6,OM:6,QA:6,SA:6,SD:6,SY:6,YE:6},weekendStart:{"001":6,AE:5,AF:4,BH:5,DZ:5,EG:5,IL:5,IN:0,IQ:5,IR:5,JO:5,KW:5,LY:5,OM:5,QA:5,SA:5,SD:5,SY:5,UG:0,YE:5}},we=864e5;function ve(t,e){const s=t.includes("-ca-")?"":"-ca-gregory",r=t.includes("-nu-")?"":"-nu-latn",n=`${t}${s||r?"-u":""}${s}${r}`;return new Intl.DateTimeFormat(n,e)}function Se(t,e){return null===t&&null===e||null!==t&&null!==e&&t.getTime()===e.getTime()}function Te(t,e){const s=xe(e);return(t.getDay()-s+7)%7}function xe(t){const e=Be(t),s=fe.firstDay[e];return void 0!==s?s:fe.firstDay["001"]}function ke(t){const e=Ee(t);return e.setDate(1),e}function Ee(t){const e=new Date(t.getTime());return e.setHours(0),e.setMinutes(0),e.setSeconds(0),e.setMilliseconds(0),e}function Pe(t){const e=new Date(t.getTime());return e.setHours(12),e.setMinutes(0),e.setSeconds(0),e.setMilliseconds(0),e}function De(t,e){const s=Pe(t);return s.setDate(s.getDate()+e),Oe(t,s),s}function Ce(t,e){const s=Pe(t);return s.setMonth(t.getMonth()+e),Oe(t,s),s}function Ae(){return Ee(new Date)}function Le(t){const e=Be(t),s=fe.weekendEnd[e];return void 0!==s?s:fe.weekendEnd["001"]}function Me(t){const e=Be(t),s=fe.weekendStart[e];return void 0!==s?s:fe.weekendStart["001"]}function Oe(t,e){e.setHours(t.getHours()),e.setMinutes(t.getMinutes()),e.setSeconds(t.getSeconds()),e.setMilliseconds(t.getMilliseconds())}function Be(t){const e=t?t.split("-"):null;return e?e[1]:"001"}function Ie(t){return class extends t{attributeChangedCallback(t,e,s){"date"===t?this.date=new Date(s):super.attributeChangedCallback(t,e,s)}get date(){return this[Mt].date}set date(t){Se(t,this[Mt].date)||this[Dt]({date:t})}get[$](){return Object.assign(super[$]||{},{date:null,locale:navigator.language})}get locale(){return this[Mt].locale}set locale(t){this[Dt]({locale:t})}[kt](t){if(super[kt]&&super[kt](t),t.date&&this[vt]){const t=this[Mt].date,e=new CustomEvent("date-changed",{bubbles:!0,detail:{date:t}});this.dispatchEvent(e);const s=new CustomEvent("datechange",{bubbles:!0,detail:{date:t}});this.dispatchEvent(s)}}}}function He(t){return class extends t{constructor(){super();!this[wt]&&this.attachInternals&&(this[wt]=this.attachInternals())}get[$](){return Object.assign(super[$]||{},{selected:!1})}[St](t){if(super[St](t),t.selected){const{selected:t}=this[Mt];k(this,"selected",t)}}[kt](t){if(super[kt]&&super[kt](t),t.selected){const{selected:t}=this[Mt],e=new CustomEvent("selected-changed",{bubbles:!0,detail:{selected:t}});this.dispatchEvent(e);const s=new CustomEvent("selectedchange",{bubbles:!0,detail:{selected:t}});this.dispatchEvent(s)}}get selected(){return this[Mt].selected}set selected(t){this[Dt]({selected:t})}}}const Ne=Ie(He(W)),Fe=class extends Ne{get[$](){return Object.assign(super[$],{date:Ae(),outsideRange:!1})}[St](t){super[St](t);const{date:e}=this[Mt];if(t.date){const t=Ae(),s=e.getDay(),r=e.getDate(),n=De(e,1),a=Math.round(e.getTime()-t.getTime())/we;k(this,"alternate-month",Math.abs(e.getMonth()-t.getMonth())%2==1),k(this,"first-day-of-month",1===r),k(this,"first-week",r<=7),k(this,"future",e>t),k(this,"last-day-of-month",e.getMonth()!==n.getMonth()),k(this,"past",e<t),k(this,"sunday",0===s),k(this,"monday",1===s),k(this,"tuesday",2===s),k(this,"wednesday",3===s),k(this,"thursday",4===s),k(this,"friday",5===s),k(this,"saturday",6===s),k(this,"today",0===a),this[pt].day.textContent=r.toString()}if(t.date||t.locale){const t=e.getDay(),{locale:s}=this[Mt],r=t===Me(s)||t===Le(s);k(this,"weekday",!r),k(this,"weekend",r)}t.outsideRange&&k(this,"outside-range",this[Mt].outsideRange)}get outsideRange(){return this[Mt].outsideRange}set outsideRange(t){this[Dt]({outsideRange:t})}get[Ut](){return C.html`
      <style>
        :host {
          box-sizing: border-box;
          display: inline-block;
        }
      </style>
      <div id="day"></div>
    `}},Re=He(he),je=Ie(class extends Re{}),Ye=class extends je{get[$](){return Object.assign(super[$],{date:Ae(),dayPartType:Fe,outsideRange:!1,tabIndex:-1})}get dayPartType(){return this[Mt].dayPartType}set dayPartType(t){this[Dt]({dayPartType:t})}get outsideRange(){return this[Mt].outsideRange}set outsideRange(t){this[Dt]({outsideRange:t})}[St](t){if(super[St](t),t.dayPartType){const{dayPartType:t}=this[Mt];U(this[pt].day,t)}const e=this[pt].day;(t.dayPartType||t.date)&&(e.date=this[Mt].date),(t.dayPartType||t.locale)&&(e.locale=this[Mt].locale),(t.dayPartType||t.outsideRange)&&(e.outsideRange=this[Mt].outsideRange),(t.dayPartType||t.selected)&&(e.selected=this[Mt].selected)}get[Ut](){const t=super[Ut],e=t.content.querySelector("slot:not([name])");if(e){const t=G(this[Mt].dayPartType);t.id="day",e.replaceWith(t)}return t.content.append(D`
        <style>
          [part~="day"] {
            width: 100%;
          }
        </style>
      `),t}},We=class extends W{get[$](){return Object.assign(super[$],{format:"short",locale:navigator.language})}get format(){return this[Mt].format}set format(t){this[Dt]({format:t})}get locale(){return this[Mt].locale}set locale(t){this[Dt]({locale:t})}[St](t){if(super[St](t),t.format||t.locale){const{format:t,locale:e}=this[Mt],s=ve(e,{weekday:t}),r=xe(e),n=Me(e),a=Le(e),o=new Date(2017,0,1),i=this[Ct].querySelectorAll('[part~="day-name"]');for(let t=0;t<=6;t++){const e=(r+t)%7;o.setDate(e+1);const l=e===n||e===a,d=i[t];d.toggleAttribute("weekday",!l),d.toggleAttribute("weekend",l),d.textContent=s.format(o)}}}get[Ut](){return C.html`
      <style>
        :host {
          direction: ltr;
          display: inline-grid;
          grid-template-columns: repeat(7, 1fr);
        }
      </style>

      <div id="day0" part="day-name"></div>
      <div id="day1" part="day-name"></div>
      <div id="day2" part="day-name"></div>
      <div id="day3" part="day-name"></div>
      <div id="day4" part="day-name"></div>
      <div id="day5" part="day-name"></div>
      <div id="day6" part="day-name"></div>
    `}},Ve=Ie(W),Ge=class extends Ve{attributeChangedCallback(t,e,s){"start-date"===t?this.startDate=new Date(s):super.attributeChangedCallback(t,e,s)}dayElementForDate(t){return(this.days||[]).find((e=>Se(e.date,t)))}get dayCount(){return this[Mt].dayCount}set dayCount(t){this[Dt]({dayCount:t})}get dayPartType(){return this[Mt].dayPartType}set dayPartType(t){this[Dt]({dayPartType:t})}get days(){return this[Mt].days}get[$](){const t=Ae();return Object.assign(super[$],{date:t,dayCount:1,dayPartType:Fe,days:null,showCompleteWeeks:!1,showSelectedDay:!1,startDate:t})}[St](t){if(super[St](t),t.days&&function(t,e){const s=[...e],r=t.childNodes.length,n=s.length,a=Math.max(r,n);for(let e=0;e<a;e++){const a=t.childNodes[e],o=s[e];e>=r?t.append(o):e>=n?t.removeChild(t.childNodes[n]):a!==o&&(s.indexOf(a,e)>=e?t.insertBefore(o,a):t.replaceChild(o,a))}}(this[pt].dayContainer,this[Mt].days),t.date||t.locale||t.showSelectedDay){const t=this[Mt].showSelectedDay,{date:e}=this[Mt],s=e.getDate(),r=e.getMonth(),n=e.getFullYear();(this.days||[]).forEach((e=>{const a=e.date,o=t&&a.getDate()===s&&a.getMonth()===r&&a.getFullYear()===n;e.toggleAttribute("selected",o)}))}if(t.dayCount||t.startDate){const{dayCount:t,startDate:e}=this[Mt],s=De(e,t);(this[Mt].days||[]).forEach((t=>{if("outsideRange"in t){const r=t.date.getTime(),n=r<e.getTime()||r>=s.getTime();t.outsideRange=n}}))}}get showCompleteWeeks(){return this[Mt].showCompleteWeeks}set showCompleteWeeks(t){this[Dt]({showCompleteWeeks:t})}get showSelectedDay(){return this[Mt].showSelectedDay}set showSelectedDay(t){this[Dt]({showSelectedDay:t})}get startDate(){return this[Mt].startDate}set startDate(t){Se(this[Mt].startDate,t)||this[Dt]({startDate:t})}[Ot](t,e){const s=super[Ot](t,e);if(e.dayCount||e.dayPartType||e.locale||e.showCompleteWeeks||e.startDate){const r=function(t,e){const{dayCount:s,dayPartType:r,locale:n,showCompleteWeeks:a,startDate:o}=t,i=a?function(t,e){return Ee(De(t,-Te(t,e)))}(o,n):Ee(o);let l;if(a){d=i,c=function(t,e){return Ee(De(t,6-Te(t,e)))}(De(o,s-1),n),l=Math.round((c.getTime()-d.getTime())/we)+1}else l=s;var d,c;let u=t.days?t.days.slice():[],h=i;for(let t=0;t<l;t++){const s=e||t>=u.length,a=s?G(r):u[t];a.date=new Date(h.getTime()),a.locale=n,"part"in a&&(a.part="day"),a.style.gridColumnStart="",s&&(u[t]=a),h=De(h,1)}l<u.length&&(u=u.slice(0,l));const p=u[0];if(p&&!a){const e=Te(p.date,t.locale);p.style.gridColumnStart=e+1}return Object.freeze(u),u}(t,e.dayPartType);Object.assign(s,{days:r})}return s}get[Ut](){return C.html`
      <style>
        :host {
          display: inline-block;
        }

        [part~="day-container"] {
          direction: ltr;
          display: grid;
          grid-template-columns: repeat(7, 1fr);
        }
      </style>

      <div id="dayContainer" part="day-container"></div>
    `}},Ue=Ie(W),Ke=class extends Ue{get[$](){return Object.assign(super[$],{date:Ae(),monthFormat:"long",yearFormat:"numeric"})}get monthFormat(){return this[Mt].monthFormat}set monthFormat(t){this[Dt]({monthFormat:t})}[St](t){if(super[St](t),t.date||t.locale||t.monthFormat||t.yearFormat){const{date:t,locale:e,monthFormat:s,yearFormat:r}=this[Mt],n={};s&&(n.month=s),r&&(n.year=r);const a=ve(e,n);this[pt].formatted.textContent=a.format(t)}}get[Ut](){return C.html`
      <style>
        :host {
          display: inline-block;
          text-align: center;
        }
      </style>
      <div id="formatted"></div>
    `}get yearFormat(){return this[Mt].yearFormat}set yearFormat(t){this[Dt]({yearFormat:t})}},ze=Ie(W);function Ze(t,e,s){if(!s||s.dayNamesHeaderPartType){const{dayNamesHeaderPartType:s}=e,r=t.getElementById("dayNamesHeader");r&&U(r,s)}if(!s||s.monthYearHeaderPartType){const{monthYearHeaderPartType:s}=e,r=t.getElementById("monthYearHeader");r&&U(r,s)}if(!s||s.monthDaysPartType){const{monthDaysPartType:s}=e,r=t.getElementById("monthDays");r&&U(r,s)}}const $e=be(Ie(Xt(function(t){return class extends t{constructor(){super();!this[wt]&&this.attachInternals&&(this[wt]=this.attachInternals())}checkValidity(){return this[wt].checkValidity()}get[$](){return Object.assign(super[$]||{},{name:"",validationMessage:"",valid:!0})}get internals(){return this[wt]}static get formAssociated(){return!0}get form(){return this[wt].form}get name(){return this[Mt]?this[Mt].name:""}set name(e){"name"in t.prototype&&(super.name=e),this[Dt]({name:e})}[St](t){if(super[St]&&super[St](t),t.name&&this.setAttribute("name",this[Mt].name),this[wt]&&this[wt].setValidity&&(t.valid||t.validationMessage)){const{valid:t,validationMessage:e}=this[Mt];t?this[wt].setValidity({}):this[wt].setValidity({customError:!0},e)}}[kt](t){super[kt]&&super[kt](t),t.value&&this[wt]&&this[wt].setFormValue(this[Mt].value,this[Mt])}reportValidity(){return this[wt].reportValidity()}get type(){return super.type||this.localName}get validationMessage(){return this[Mt].validationMessage}get validity(){return this[wt].validity}get willValidate(){return this[wt].willValidate}}}(function(t){return class extends t{[et](){if(super[et])return super[et]()}[st](){if(super[st])return super[st]()}[at](){if(super[at])return super[at]()}[lt](){if(super[lt])return super[lt]()}[dt](){if(super[dt])return super[dt]()}[ut](){if(super[ut])return super[ut]()}[gt](t){let e=!1;const s=this[Mt].orientation||"both",r="horizontal"===s||"both"===s,n="vertical"===s||"both"===s;switch(t.key){case"ArrowDown":n&&(e=t.altKey?this[st]():this[et]());break;case"ArrowLeft":!r||t.metaKey||t.altKey||(e=this[at]());break;case"ArrowRight":!r||t.metaKey||t.altKey||(e=this[lt]());break;case"ArrowUp":n&&(e=t.altKey?this[dt]():this[ut]());break;case"End":e=this[st]();break;case"Home":e=this[dt]()}return e||super[gt]&&super[gt](t)||!1}}}(function(t){return class extends t{constructor(){super(),this.addEventListener("keydown",(async t=>{this[vt]=!0,this[Mt].focusVisible||this[Dt]({focusVisible:!0}),this[gt](t)&&(t.preventDefault(),t.stopImmediatePropagation()),await Promise.resolve(),this[vt]=!1}))}attributeChangedCallback(t,e,s){if("tabindex"===t){let t;null===s?t=-1:(t=Number(s),isNaN(t)&&(t=this[q]?this[q]:0)),this.tabIndex=t}else super.attributeChangedCallback(t,e,s)}get[$](){const t=this[J]?-1:0;return Object.assign(super[$]||{},{tabIndex:t})}[gt](t){return!!super[gt]&&super[gt](t)}[St](t){super[St]&&super[St](t),t.tabIndex&&(this.tabIndex=this[Mt].tabIndex)}get tabIndex(){return super.tabIndex}set tabIndex(t){super.tabIndex!==t&&(super.tabIndex=t),this[Et]||this[Dt]({tabIndex:t})}}}(function(t){return class extends t{connectedCallback(){const t="rtl"===getComputedStyle(this).direction;this[Dt]({rightToLeft:t}),super.connectedCallback()}}}(class extends ze{dayElementForDate(t){const e=this[pt].monthDays;return e&&"dayElementForDate"in e&&e.dayElementForDate(t)}get dayNamesHeaderPartType(){return this[Mt].dayNamesHeaderPartType}set dayNamesHeaderPartType(t){this[Dt]({dayNamesHeaderPartType:t})}get dayPartType(){return this[Mt].dayPartType}set dayPartType(t){this[Dt]({dayPartType:t})}get days(){return this[Ct]?this[pt].monthDays.days:[]}get daysOfWeekFormat(){return this[Mt].daysOfWeekFormat}set daysOfWeekFormat(t){this[Dt]({daysOfWeekFormat:t})}get[$](){return Object.assign(super[$],{date:Ae(),dayNamesHeaderPartType:We,dayPartType:Fe,daysOfWeekFormat:"short",monthDaysPartType:Ge,monthFormat:"long",monthYearHeaderPartType:Ke,showCompleteWeeks:!1,showSelectedDay:!1,yearFormat:"numeric"})}get monthFormat(){return this[Mt].monthFormat}set monthFormat(t){this[Dt]({monthFormat:t})}get monthDaysPartType(){return this[Mt].monthDaysPartType}set monthDaysPartType(t){this[Dt]({monthDaysPartType:t})}get monthYearHeaderPartType(){return this[Mt].monthYearHeaderPartType}set monthYearHeaderPartType(t){this[Dt]({monthYearHeaderPartType:t})}[St](t){if(super[St](t),Ze(this[Ct],this[Mt],t),(t.dayPartType||t.monthDaysPartType)&&(this[pt].monthDays.dayPartType=this[Mt].dayPartType),t.locale||t.monthDaysPartType||t.monthYearHeaderPartType||t.dayNamesHeaderPartType){const t=this[Mt].locale;this[pt].monthDays.locale=t,this[pt].monthYearHeader.locale=t,this[pt].dayNamesHeader.locale=t}if(t.date||t.monthDaysPartType){const{date:t}=this[Mt];if(t){const e=ke(t),s=function(t){const e=ke(t);return e.setMonth(e.getMonth()+1),e.setDate(e.getDate()-1),e}(t).getDate();Object.assign(this[pt].monthDays,{date:t,dayCount:s,startDate:e}),this[pt].monthYearHeader.date=ke(t)}}if(t.daysOfWeekFormat||t.dayNamesHeaderPartType){const{daysOfWeekFormat:t}=this[Mt];this[pt].dayNamesHeader.format=t}if(t.showCompleteWeeks||t.monthDaysPartType){const{showCompleteWeeks:t}=this[Mt];this[pt].monthDays.showCompleteWeeks=t}if(t.showSelectedDay||t.monthDaysPartType){const{showSelectedDay:t}=this[Mt];this[pt].monthDays.showSelectedDay=t}if(t.monthFormat||t.monthYearHeaderPartType){const{monthFormat:t}=this[Mt];this[pt].monthYearHeader.monthFormat=t}if(t.yearFormat||t.monthYearHeaderPartType){const{yearFormat:t}=this[Mt];this[pt].monthYearHeader.yearFormat=t}}get showCompleteWeeks(){return this[Mt].showCompleteWeeks}set showCompleteWeeks(t){this[Dt]({showCompleteWeeks:t})}get showSelectedDay(){return this[Mt].showSelectedDay}set showSelectedDay(t){this[Dt]({showSelectedDay:t})}get[Ut](){const t=C.html`
      <style>
        :host {
          display: inline-block;
        }

        [part~="month-year-header"] {
          display: block;
        }

        [part~="day-names-header"] {
          display: grid;
        }

        [part~="month-days"] {
          display: block;
        }
      </style>

      <div id="monthYearHeader" part="month-year-header"></div>
      <div
        id="dayNamesHeader"
        part="day-names-header"
        exportparts="day-name"
        format="short"
      ></div>
      <div id="monthDays" part="month-days" exportparts="day"></div>
    `;return Ze(t.content,this[Mt]),t}get yearFormat(){return this[Mt].yearFormat}set yearFormat(t){this[Dt]({yearFormat:t})}}))))))),qe=class extends $e{constructor(){super(),this.addEventListener("mousedown",(t=>{if(0!==t.button)return;this[vt]=!0;const e=t.composedPath()[0];if(e instanceof Node){const t=this.days,s=t[function(t,e){return Array.prototype.findIndex.call(t,(t=>t===e||S(t,e)))}(t,e)];s&&(this.date=s.date)}this[vt]=!1})),x(this,this)}arrowButtonNext(){const t=this[Mt].date||Ae();return this[Dt]({date:Ce(t,1)}),!0}arrowButtonPrevious(){const t=this[Mt].date||Ae();return this[Dt]({date:Ce(t,-1)}),!0}get[$](){return Object.assign(super[$],{arrowButtonOverlap:!1,canGoNext:!0,canGoPrevious:!0,date:Ae(),dayPartType:Ye,orientation:"both",showCompleteWeeks:!0,showSelectedDay:!0,value:null})}[gt](t){let e=!1;switch(t.key){case"Home":this[Dt]({date:Ae()}),e=!0;break;case"PageDown":this[Dt]({date:Ce(this[Mt].date,1)}),e=!0;break;case"PageUp":this[Dt]({date:Ce(this[Mt].date,-1)}),e=!0}return e||super[gt]&&super[gt](t)}[et](){return super[et]&&super[et](),this[Dt]({date:De(this[Mt].date,7)}),!0}[at](){return super[at]&&super[at](),this[Dt]({date:De(this[Mt].date,-1)}),!0}[lt](){return super[lt]&&super[lt](),this[Dt]({date:De(this[Mt].date,1)}),!0}[ut](){return super[ut]&&super[ut](),this[Dt]({date:De(this[Mt].date,-7)}),!0}[Ot](t,e){const s=super[Ot](t,e);return e.date&&Object.assign(s,{value:t.date?t.date.toString():""}),s}get[Ut](){const t=super[Ut],e=t.content.querySelector("#monthYearHeader");this[be.wrap](e);const s=C.html`
      <style>
        [part~="arrow-icon"] {
          font-size: 24px;
        }
      </style>
    `;return t.content.append(s.content),t}get value(){return this.date}set value(t){this.date=t}},Je=new Set;function Qe(t){return class extends t{attributeChangedCallback(t,e,s){if("dark"===t){const e=w(t,s);this.dark!==e&&(this.dark=e)}else super.attributeChangedCallback(t,e,s)}disconnectedCallback(){super.disconnectedCallback&&super.disconnectedCallback(),Je.delete(this)}get dark(){return this[Mt].dark}set dark(t){this[Dt]({dark:t})}get[$](){return Object.assign(super[$]||{},{dark:!1,detectDarkMode:"auto"})}get detectDarkMode(){return this[Mt].detectDarkMode}set detectDarkMode(t){"auto"!==t&&"off"!==t||this[Dt]({detectDarkMode:t})}[St](t){if(super[St]&&super[St](t),t.dark){const{dark:t}=this[Mt];k(this,"dark",t)}}[kt](t){if(super[kt]&&super[kt](t),t.detectDarkMode){const{detectDarkMode:t}=this[Mt];"auto"===t?(Je.add(this),Xe(this)):Je.delete(this)}}}}function Xe(t){const e=function(t){const e=/rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*[\d.]+\s*)?\)/.exec(t);return e?{r:e[1],g:e[2],b:e[3]}:null}(_e(t));if(e){const s=function(t){const e=t.r/255,s=t.g/255,r=t.b/255,n=Math.max(e,s,r),a=Math.min(e,s,r);let o=0,i=0,l=(n+a)/2;const d=n-a;if(0!==d){switch(i=l>.5?d/(2-d):d/(n+a),n){case e:o=(s-r)/d+(s<r?6:0);break;case s:o=(r-e)/d+2;break;case r:o=(e-s)/d+4}o/=6}return{h:o,s:i,l}}(e).l<.5;t[Dt]({dark:s})}}function _e(t){const e="rgb(255,255,255)";if(t instanceof Document)return e;const s=getComputedStyle(t).backgroundColor;if(s&&"transparent"!==s&&"rgba(0, 0, 0, 0)"!==s)return s;if(t.assignedSlot)return _e(t.assignedSlot);const r=t.parentNode;return r instanceof ShadowRoot?_e(r.host):r instanceof Element?_e(r):e}window.matchMedia("(prefers-color-scheme: dark)").addListener((()=>{Je.forEach((t=>{Xe(t)}))}));class ts extends(function(t){return class extends t{get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host([disabled]) ::slotted(*) {
            opacity: 0.5;
          }

          [part~="inner"] {
            display: inline-flex;
            justify-content: center;
            margin: 0;
            position: relative;
          }
        </style>
      `),t}}}(he)){}const es=Qe(ts),ss=class extends es{get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            color: rgba(0, 0, 0, 0.7);
          }

          :host(:not([disabled]):hover) {
            background: rgba(0, 0, 0, 0.2);
            color: rgba(0, 0, 0, 0.8);
            cursor: pointer;
          }

          :host([disabled]) {
            color: rgba(0, 0, 0, 0.3);
          }

          [part~="inner"] {
            fill: currentcolor;
          }

          :host([dark]) {
            color: rgba(255, 255, 255, 0.7);
          }

          :host([dark]:not([disabled]):hover) {
            background: rgba(255, 255, 255, 0.2);
            color: rgba(255, 255, 255, 0.8);
          }

          :host([dark][disabled]) {
            color: rgba(255, 255, 255, 0.3);
          }
        </style>
      `),t}},rs=class extends Fe{get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            padding: 0.3em;
            text-align: right;
          }

          :host([weekend]) {
            color: gray;
          }

          :host([outside-range]) {
            color: lightgray;
          }

          :host([today]) {
            color: darkred;
            font-weight: bold;
          }

          :host([selected]) {
            background: #ddd;
          }
        </style>
      `),t}},ns=class extends Ye{get[$](){return Object.assign(super[$],{dayPartType:rs})}get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            border: 1px solid transparent;
          }

          :host(:hover) {
            border-color: gray;
          }

          :host([selected]) {
            background: #ddd;
          }
        </style>
      `),t}},as=class extends We{get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            font-size: smaller;
          }

          [part~="day-name"] {
            padding: 0.3em;
            text-align: center;
            white-space: nowrap;
          }

          [weekend] {
            color: gray;
          }
        </style>
      `),t}},os=class extends Ke{get[Ut](){const t=super[Ut];return t.content.append(D`
        <style>
          :host {
            font-size: larger;
            font-weight: bold;
            padding: 0.3em;
          }
        </style>
      `),t}};class is extends(Qe(function(t){return class extends t{get[$](){return Object.assign(super[$]||{},{arrowButtonPartType:ss})}[St](t){if(super[St](t),t.orientation||t.rightToLeft){const{orientation:t,rightToLeft:e}=this[Mt],s="vertical"===t?"rotate(90deg)":e?"rotateZ(180deg)":"";this[pt].arrowIconPrevious&&(this[pt].arrowIconPrevious.style.transform=s),this[pt].arrowIconNext&&(this[pt].arrowIconNext.style.transform=s)}if(t.dark){const{dark:t}=this[Mt],e=this[pt].arrowButtonPrevious,s=this[pt].arrowButtonNext;"dark"in e&&(e.dark=t),"dark"in s&&(s.dark=t)}}get[Ut](){const t=super[Ut],e=t.content.querySelector('slot[name="arrowButtonPrevious"]');e&&e.append(D`
            <svg
              id="arrowIconPrevious"
              part="arrow-icon arrow-icon-previous"
              viewBox="0 0 24 24"
              preserveAspectRatio="xMidYMid meet"
              style="fill: currentColor; height: 1em; width: 1em;"
            >
              <g>
                <path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"></path>
              </g>
            </svg>
          `);const s=t.content.querySelector('slot[name="arrowButtonNext"]');return s&&s.append(D`
            <svg
              id="arrowIconNext"
              part="arrow-icon arrow-icon-next"
              viewBox="0 0 24 24"
              preserveAspectRatio="xMidYMid meet"
              style="fill: currentColor; height: 1em; width: 1em;"
            >
              <g>
                <path d="M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z"></path>
              </g>
            </svg>
          `),t}}}(qe))){get[$](){return Object.assign(super[$],{dayNamesHeaderPartType:as,dayPartType:ns,monthYearHeaderPartType:os})}}const ls=is;customElements.define("elix-calendar-month-navigator",class extends ls{})})();