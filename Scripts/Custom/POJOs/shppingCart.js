function shoppingCart() {
    var self = this;
    var cartContents = [];
    var totalItemsInCart = 0;
    var totalCartPrice = 0;

    
    self.addToCart = function (id, name, price) {

        totalCartPrice += price;
        totalItemsInCart += 1;

        for (i = 0; i < cartContents.length; i++) {
            if (cartContents[i].productId == id) {
                cartContents[i].count += 1;
                return;
            }
        }

        cartContents.push({ count:1, productId:id, price:price, name:name });
    }

    self.removeFromCart = function (id) {
        for (i = 0; i < cartContents.length; i++) {
            if (cartContents[i].productId == id) {
                totalItemsInCart -= cartContents[i].count;
                totalCartPrice -= (cartContents[i] * cartContents[i].count);
                cartContents.splice(i, 1);
                break;
            }
        }
    }

    self.getCartItemsCount = function () {
        return totalItemsInCart;
    }

    self.getCartPrice = function () {
        return totalCartPrice;
    }

    self.getProducts = function () {
        return cartContents;
    }

    self.emptyCart = function () {
        cartContents.length = 0;
        totalCartPrice = 0;
        totalItemsInCart = 0;
    }
}